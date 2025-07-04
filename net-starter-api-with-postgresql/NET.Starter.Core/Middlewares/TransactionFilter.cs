using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using System.Net;

namespace NET.Starter.Core.Middlewares
{
    /// <summary>
    /// Middleware that wraps an API action within a database transaction scope.
    /// It commits the transaction if the result matches accepted status codes; otherwise, rolls it back.
    /// </summary>
    /// <typeparam name="TApplicationDbContext">The application's DbContext type.</typeparam>
    internal class TransactionFilter<TApplicationDbContext>(TApplicationDbContext _dbContext, ILogger<TransactionFilter<TApplicationDbContext>> _logger) : IAsyncActionFilter
        where TApplicationDbContext : DbContext
    {
        /// <summary>
        /// Executes the action within a transaction, committing or rolling back based on result status code.
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var mutationAttribute = context.ActionDescriptor.EndpointMetadata.OfType<MutationAttribute>().FirstOrDefault();

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _logger.LogInformation("Database transaction started for action: {Action}", context.ActionDescriptor.DisplayName);

                var resultContext = await next();

                if (resultContext.Exception == null)
                {
                    switch (resultContext.Result)
                    {
                        case ObjectResult objectResult:
                            await HandleObjectResult(context, transaction, mutationAttribute, objectResult);
                            break;

                        case FileStreamResult fileStreamResult:
                            await transaction.RollbackAsync();
                            _logger.LogInformation("Transaction rolled back for FileStreamResult (read-only operation)");
                            break;

                        default:
                            await transaction.RollbackAsync();
                            _logger.LogWarning("Transaction rolled back: Unrecognized result type from action.");
                            break;
                    }
                }
                else
                {
                    await RollbackTransaction(transaction, "Exception thrown during action execution.");
                    HandleUnexpectedException(context, resultContext.Exception, "An exception occurred during action execution.");

                    resultContext.Result = context.Result;
                    resultContext.Exception = null;
                }
            }
            catch (Exception ex)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await RollbackTransaction(_dbContext.Database.CurrentTransaction, "Unhandled exception in transaction filter.");

                HandleUnexpectedException(context, ex, "Unhandled error during transaction handling.");
            }
        }

        /// <summary>
        /// Handles the logic for an ObjectResult, including validation of accepted response codes and transaction finalization.
        /// </summary>
        private async Task HandleObjectResult(ActionExecutingContext context, IDbContextTransaction transaction, MutationAttribute? mutationAttribute, ObjectResult objectResult)
        {
            if (objectResult.Value is not BaseDto baseDto)
            {
                await transaction.RollbackAsync();
                _logger.LogWarning("Transaction rolled back: Result does not implement BaseDto.");
                return;
            }

            // Ensure response carries proper HTTP status code and trace ID
            context.HttpContext.Response.StatusCode = baseDto.Code;
            baseDto.Id = context.HttpContext.TraceIdentifier;

            if (mutationAttribute != null)
            {
                if (mutationAttribute.AcceptedResponseCodes.Contains(baseDto.Code))
                {
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transaction committed. Status code accepted: {Code}", baseDto.Code);
                }
                else
                {
                    await transaction.RollbackAsync();
                    _logger.LogWarning("Transaction rolled back. Unexpected status code: {Code}", baseDto.Code);
                }
            }
            else
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Transaction rolled back. Endpoint is not marked as mutation.");
            }
        }

        /// <summary>
        /// Rolls back the current transaction with a reason for logging.
        /// </summary>
        private async Task RollbackTransaction(IDbContextTransaction transaction, string reason)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Transaction rolled back. Reason: {Reason}", reason);
            }
        }

        /// <summary>
        /// Constructs and logs an error response for unexpected exceptions.
        /// </summary>
        private void HandleUnexpectedException(ActionExecutingContext context, Exception ex, string errorMessage)
        {
            var errorResponse = new BaseDto($"An error occurred. Trace ID: {context.HttpContext.TraceIdentifier}", HttpStatusCode.InternalServerError)
            {
                Id = context.HttpContext.TraceIdentifier
            };

            context.Result = new JsonResult(errorResponse);
            context.HttpContext.Response.StatusCode = errorResponse.Code;

            _logger.LogError(ex, "{Message}", errorMessage);
        }
    }
}
