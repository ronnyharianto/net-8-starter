using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NET.Starter.API.Shared.Attributes;
using NET.Starter.API.Shared.Enums;
using NET.Starter.API.Shared.Objects.Dtos;

namespace NET.Starter.API.Middlewares
{
    public class TransactionFilter<TApplicationDbContext>(TApplicationDbContext dbContext, ILogger<TransactionFilter<TApplicationDbContext>> logger) : IAsyncActionFilter
        where TApplicationDbContext : DbContext
    {
        private readonly TApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<TransactionFilter<TApplicationDbContext>> _logger = logger;
        private readonly string logPrefix = nameof(TransactionFilter<TApplicationDbContext>);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the method has the MutationAttribute
            bool isMutation = context.ActionDescriptor.EndpointMetadata.OfType<MutationAttribute>().Any();
            bool isCustomResponse = context.ActionDescriptor.EndpointMetadata.OfType<CustomResponseAttribute>().Any();

            _logger.LogInformation("{Prefix}: Open transaction scope", logPrefix);
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var resultContext = await next();

                if (resultContext.Result != null && resultContext.Exception == null)
                {
                    var objectResult = (ObjectResult)resultContext.Result;

                    if (objectResult.Value is not ResponseBase && !isCustomResponse)
                    {
                        var errorMessage = "Use ResponseBase or it's inheritance";
                        resultContext.Result = new JsonResult(new ResponseBase(errorMessage, ResponseCode.Error)
                        {
                            Id = context.HttpContext.TraceIdentifier
                        });

                        throw new InvalidOperationException(errorMessage);
                    }

                    if (objectResult.Value is ResponseBase @baseSetId) @baseSetId.Id = context.HttpContext.TraceIdentifier;

                    if (isMutation && objectResult.Value != null)
                    {
                        if (objectResult.Value is ResponseBase @baseCheckSucceeded && @baseCheckSucceeded.Succeeded || isCustomResponse)
                        {
                            _logger.LogInformation("{Prefix}: Commit transaction scope", logPrefix);
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            var statusCode = objectResult.Value is ResponseBase @baseGetStatusCode ? @baseGetStatusCode.Code.ToString() : "400";

                            _logger.LogError("{Prefix}: Rollback transaction scope: Status code {StatusCode}", logPrefix, statusCode);
                            await transaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
                else
                {
                    var exceptionMessage = resultContext.Exception?.Message;

                    resultContext.Result = new JsonResult(new ResponseBase(exceptionMessage, ResponseCode.Error)
                    {
                        Id = context.HttpContext.TraceIdentifier
                    });

                    _logger.LogError("{Prefix}: Stack Trace: {StackTrace}", logPrefix, resultContext.Exception?.StackTrace?.Trim());

                    resultContext.Exception = null;
                    throw new InvalidOperationException(exceptionMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{Prefix}: Rollback transaction scope: {Message}", logPrefix, ex.Message);
                await transaction.RollbackAsync();
            }
        }
    }
}
