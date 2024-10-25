namespace FSTD.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Log the incoming request
                var request = context.Request;
                var requestMessage = $"{request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                _logger.LogInformation($"Received request: {requestMessage}");

                // Call the next middleware in the pipeline
                await _next(context);
                _logger.LogInformation($"Response status code: {context.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
