using FSTD.ExceptionsHandler.Contexts;
using FSTD.Exeptions.Models.AzureExceptions;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Exeptions.Models.ObjectsExceptions;
using FSTD.Exeptions.Models.ServicesExeptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FSTD.ExceptionsHandler.Exceptions
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<BadRequestException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.BadRequest, "Bad request.");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<UnauthorizedAccessException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            catch (ValidationException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<ValidationException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.BadRequest, "Validation Error");
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<NotFoundException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotFound, "The request with its parameters has not found.");
            }
            catch (ConflictException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<ConflictException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.Conflict, "User already exists!");
            }
            catch (TaskCanceledException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<TaskCanceledException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.RequestTimeout
                    , e.Message);
            }
            catch (BlobUploadException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<BlobUploadException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotImplemented
                    , e.Message);
            }
            catch (InvalidDataException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<InvalidDataException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotImplemented
                    , e.Message);
            }
            catch (OverflowException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<OverflowException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotImplemented
                    , e.Message);
            }
            catch (DateFormatException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<DateFormatException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotImplemented
                    , e.Message);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<InvalidOperationException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.NotImplemented
                    , e.Message);
            }
            catch (EmailServiceException e)
            {
                _logger.LogWarning(LoggingMessageBuilder<EmailServiceException>(e.Message));
                await context.CreateExceptionHttpContext(HttpStatusCode.InternalServerError
                    , e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingMessageBuilder<Exception>(e.Message) + $" ErrorStackTrace: {e.StackTrace}");
                await context.CreateExceptionHttpContext(HttpStatusCode.InternalServerError
                    , "An error occurred while processing your request.");
            }
            string LoggingMessageBuilder<T>(string message)
            => $"{{TypeOf:{typeof(T).Name}, ErrorMessage: {message}, DateTime: {DateTime.Now}}}";
        }
    }
}
