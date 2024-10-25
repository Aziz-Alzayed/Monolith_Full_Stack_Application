using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FSTD.API.ApiAttributes
{
    public class ApiKeyAuthAttribute : ActionFilterAttribute
    {
        private const string APIKEYNAME = "X-API-KEY";
        private readonly IConfiguration _configuration;

        public ApiKeyAuthAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "API Key was not provided."
                };
                return;
            }

            var apiKey = _configuration.GetValue<string>("SeedApiKey");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Unauthorized client."
                };
                return;
            }

            await next();
        }
    }
}
