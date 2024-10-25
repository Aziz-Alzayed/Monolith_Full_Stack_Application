using MediatR;
using Microsoft.Extensions.Logging;

namespace FSTD.Infrastructure.MediatoR.Common.Decorators
{
    public class RequestsDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<RequestsDecorator<TRequest, TResponse>> _logger;

        public RequestsDecorator(ILogger<RequestsDecorator<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogInformation($"Request name:{requestName}, Start DateTime:{DateTime.Now}");

                TResponse response;

                response = await next();

                _logger.LogInformation($"Request name:{requestName}, End DateTime:{DateTime.Now}");


                return response;
            }
            catch (Exception ex) when (ex is TaskCanceledException)
            {
                throw new TaskCanceledException($"The task: {typeof(TRequest).Name} was cancelled");
            }
        }
    }
}
