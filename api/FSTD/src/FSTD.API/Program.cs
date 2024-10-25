
using FSTD.API.ApiAttributes;
using FSTD.API.Middlewares;
using FSTD.ExceptionsHandler.Exceptions;
using FSTD.Infrastructure;

var app = CreateWebApplicationBuilder().Build();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // In development mode, allow all origins, headers, and methods
    app.UseCors(builder =>
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSTD API V1");
        app.InitializeDatabase(); // Ensure database is initialized in development
    });
}
else
{
    // In other environments, use the configured allowed origins
    var allowedOrigins = app.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    app.UseCors(builder =>
        builder.WithOrigins(allowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader());
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();

WebApplicationBuilder CreateWebApplicationBuilder()
{
    var builder = WebApplication.CreateBuilder(args);
    Logging(builder);
    Services(builder);
    Configuration(builder);
    return builder;
}

void Configuration(WebApplicationBuilder builder)
{
    builder.Services.AddInfrastructure(builder.Configuration);
    // Additional configuration if needed
}

void Logging(WebApplicationBuilder builder)
{
    builder.Logging
        .ClearProviders()
        .AddConsole();
}

void Services(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<ApiKeyAuthAttribute>();
}

public partial class Program { }