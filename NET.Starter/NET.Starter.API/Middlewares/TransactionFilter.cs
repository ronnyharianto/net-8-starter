using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using System.Net;

namespace NET.Starter.API.Middlewares
{
    /// <summary>
    /// Middleware filter to handle transaction logic for incoming requests.
    /// Automatically begins a database transaction for each request,
    /// commits the transaction if the request is successful,
    /// or rolls back the transaction if an error occurs.
    /// </summary>
    /// <typeparam name="TApplicationDbContext">The type of the application's DbContext.</typeparam>
    public class TransactionFilter<TApplicationDbContext>(TApplicationDbContext dbContext, ILogger<TransactionFilter<TApplicationDbContext>> logger) : IAsyncActionFilter
        where TApplicationDbContext : DbContext
    {
        private readonly TApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<TransactionFilter<TApplicationDbContext>> _logger = logger;

        /// <summary>
        /// Executes the action within a database transaction scope.
        /// </summary>
        /// <param name="context">The action context.</param>
        /// <param name="next">The delegate to execute the next filter or action.</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isMutation = context.ActionDescriptor.EndpointMetadata.OfType<MutationAttribute>().Any(); // Indicates state-changing in database operations.
            bool isCustomResponse = context.ActionDescriptor.EndpointMetadata.OfType<CustomResponseAttribute>().Any(); // Indicates response use custom response.

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _logger.LogInformation("Database transaction scope opened");

                var resultContext = await next();

                // Check if the result exists and no exception occurred during action execution.
                if (resultContext.Exception == null)
                {
                    switch (resultContext.Result)
                    {
                        case ObjectResult objectResult:
                            {
                                BaseDto? baseDto = null;

                                if (objectResult.Value is BaseDto tempDto)
                                {
                                    baseDto = tempDto;
                                    
                                    resultContext.HttpContext.Response.StatusCode = baseDto.Code; // Assign the status code from the response DTO if it exists.
                                    baseDto.Id = context.HttpContext.TraceIdentifier; // Assign a unique trace identifier to the response DTO.
                                }
                                else if (!isCustomResponse)
                                {
                                    // Enforce that the result must inherit from BaseDto if CustomResponseAttribute is not used.
                                    var errorMessage = "Use ResponseBase or its inheritance";
                                    resultContext.Result = new JsonResult(new BaseDto(errorMessage, ResponseCode.Error)
                                    {
                                        Id = context.HttpContext.TraceIdentifier
                                    });

                                    throw new InvalidOperationException(errorMessage);
                                }

                                // Handle mutations (state-changing operations).
                                if (isMutation)
                                {
                                    if (baseDto?.Succeeded ?? false || isCustomResponse) 
                                    {
                                        await transaction.CommitAsync();
                                        _logger.LogInformation("Operation succeeded. Database transaction scope committed");
                                    }
                                    else
                                    {
                                        await transaction.RollbackAsync();
                                        _logger.LogError("Operation failed. Database transaction scope rollbacked");
                                    }
                                }
                                else
                                {
                                    await transaction.RollbackAsync();
                                    _logger.LogError("Non-mutation endpoint. Database transaction scope rollbacked");
                                }

                                break;
                            }

                        default:
                            {
                                // If not expected result, rollback the transaction
                                await transaction.RollbackAsync();
                                _logger.LogError("Unexpected result. Database transaction scope rollbacked");

                                break;
                            }
                    };
                }
                else
                {
                    await RollbackTransaction(transaction, "Unexpected exception from action");

                    HandleUnexpectedException(context, resultContext.Exception, "Error occured from action");

                    resultContext.Result = context.Result;
                    resultContext.Exception = null;
                }
            }
            catch (Exception ex)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await RollbackTransaction(_dbContext.Database.CurrentTransaction, "Unexpected exception");
                }

                HandleUnexpectedException(context, ex, "Error occured");
            }
        }

        private async Task RollbackTransaction(IDbContextTransaction transaction, string reason)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Database transaction scope rolled back: {Reason}", reason);
            }
        }

        private void HandleUnexpectedException(ActionExecutingContext context, Exception ex, string errorMessage)
        {
            var errorResponse = new BaseDto($"An error occurred. Trace ID: {context.HttpContext.TraceIdentifier}", ResponseCode.Error)
            {
                Id = context.HttpContext.TraceIdentifier
            };

            context.Result = new JsonResult(errorResponse);
            context.HttpContext.Response.StatusCode = errorResponse.Code;

            _logger.LogError(ex, "{Message}", errorMessage);
        }
    }
}
