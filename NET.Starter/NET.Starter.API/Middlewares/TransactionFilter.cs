using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.API.Middlewares
{
    /// <summary>
    /// Middleware filter to handle transaction logic for incoming requests.
    /// Automatically begins a database transaction for each request,
    /// commits the transaction if the request is successful,
    /// or rolls back the transaction if an error occurs.
    /// </summary>
    /// <typeparam name="TApplicationDbContext">
    /// The type of the application's DbContext, which must inherit from <see cref="DbContext"/>.
    /// </typeparam>
    /// <param name="dbContext">
    /// The application's database context used for managing database transactions.
    /// </param>
    /// <param name="logger">
    /// The logger instance for logging transaction activities and errors.
    /// </param>
    public class TransactionFilter<TApplicationDbContext>(TApplicationDbContext dbContext, ILogger<TransactionFilter<TApplicationDbContext>> logger) : IAsyncActionFilter
        where TApplicationDbContext : DbContext
    {
        // Initialize private readonly fields for the DbContext and Logger.
        private readonly TApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<TransactionFilter<TApplicationDbContext>> _logger = logger;
        private readonly string logPrefix = nameof(TransactionFilter<TApplicationDbContext>);

        // Intercept and execute the action with a transaction scope.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the action method is marked with MutationAttribute (indicates state-changing operations).
            bool isMutation = context.ActionDescriptor.EndpointMetadata.OfType<MutationAttribute>().Any();

            // Check if the action method is marked with CustomResponseAttribute (indicates custom response behavior).
            bool isCustomResponse = context.ActionDescriptor.EndpointMetadata.OfType<CustomResponseAttribute>().Any();

            // Log that a transaction scope is being opened.
            _logger.LogInformation("{Prefix}: Open transaction scope", logPrefix);

            // Begin a new database transaction.
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Execute the action method and capture the result context.
                var resultContext = await next();

                // Check if the result exists and no exception occurred during action execution.
                if (resultContext.Result != null && resultContext.Exception == null)
                {
                    // Cast the result to ObjectResult to inspect its value.
                    var objectResult = (ObjectResult)resultContext.Result;

                    // Enforce that the result must inherit from BaseDto if CustomResponseAttribute is not used.
                    if (objectResult.Value is not BaseDto && !isCustomResponse)
                    {
                        var errorMessage = "Use ResponseBase or its inheritance";
                        resultContext.Result = new JsonResult(new BaseDto(errorMessage, ResponseCode.Error)
                        {
                            Id = context.HttpContext.TraceIdentifier
                        });

                        throw new InvalidOperationException(errorMessage);
                    }

                    // Assign a unique trace identifier to the response DTO.
                    if (objectResult.Value is BaseDto @baseSetId) @baseSetId.Id = context.HttpContext.TraceIdentifier;

                    // Handle mutations (state-changing operations).
                    if (isMutation && objectResult.Value != null)
                    {
                        // Commit the transaction if the operation succeeded.
                        if (objectResult.Value is BaseDto @baseCheckSucceeded && @baseCheckSucceeded.Succeeded || isCustomResponse)
                        {
                            _logger.LogInformation("{Prefix}: Commit transaction scope", logPrefix);
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            // Rollback the transaction if the operation failed.
                            var statusCode = objectResult.Value is BaseDto @baseGetStatusCode ? @baseGetStatusCode.Code.ToString() : "400";

                            _logger.LogError("{Prefix}: Rollback transaction scope: Status code {StatusCode}", logPrefix, statusCode);
                            await transaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        // Rollback for non-mutation endpoints.
                        await transaction.RollbackAsync();
                    }
                }
                else
                {
                    // Handle exceptions by returning an error response.
                    var exceptionMessage = resultContext.Exception?.Message;

                    resultContext.Result = new JsonResult(new BaseDto(exceptionMessage, ResponseCode.Error)
                    {
                        Id = context.HttpContext.TraceIdentifier
                    });

                    // Log the exception stack trace for debugging.
                    _logger.LogError("{Prefix}: Stack Trace: {StackTrace}", logPrefix, resultContext.Exception?.StackTrace?.Trim());

                    // Clear the exception to prevent it from propagating further.
                    resultContext.Exception = null;
                    throw new InvalidOperationException(exceptionMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions and roll back the transaction.
                _logger.LogError("{Prefix}: Rollback transaction scope: {Message}", logPrefix, ex.Message);
                await transaction.RollbackAsync();
            }
        }
    }
}
