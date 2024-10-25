using FSTD.DataCore.Models;
using FSTD.Infrastructure.CommonServices.SeedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.API.Functional.Tests.AuthRequestsSetup
{
    public class CustomWebApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureTestServices(services =>
            {
                // Remove existing DbContext options if any
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database context for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");  // Use in-memory database
                });

                // Register the seed service and other services
                services.AddScoped<IIdentitySeedService, IdentitySeedService>();

                // Register authentication and authorization for tests
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

                services.AddAuthorization(options =>
                {
                    var defaultPolicy = new AuthorizationPolicyBuilder(TestAuthHandler.SchemeName)
                        .RequireAuthenticatedUser()
                        .Build();
                    options.DefaultPolicy = defaultPolicy;
                });

                services.AddScoped(_ => new AuthClaimsProvider());
            });

            // Perform database seeding after services are configured
            builder.ConfigureServices(async services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var seedService = scope.ServiceProvider.GetRequiredService<IIdentitySeedService>();

                    // No need to call EnsureCreated() for in-memory databases
                    await SeedDatabaseAsync(seedService);
                }
            });
        }

        private async Task SeedDatabaseAsync(IIdentitySeedService seedService)
        {
            // Seed data asynchronously
            await seedService.SeedRoles();
            await seedService.SeedSuperUser();
            await seedService.SeedAdminUser();
            await seedService.SeedUser();
        }
    }
}
