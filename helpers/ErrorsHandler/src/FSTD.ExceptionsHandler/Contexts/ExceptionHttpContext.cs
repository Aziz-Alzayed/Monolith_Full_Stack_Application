using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace FSTD.ExceptionsHandler.Contexts
{
    public static class ExceptionHttpContext
    {
        public static async Task CreateExceptionHttpContext(this HttpContext context, HttpStatusCode httpStatusCode, string message)
        {
            // Return a custom error response
            var response = new { message };
            var jsonResponse = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
