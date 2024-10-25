using FSTD.DataCore.Models;
using FSTD.DataCore.Models.JwtModels;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.Authentication;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using FSTD.Infrastructure.CommonServices.SeedServices;
using FSTD.Infrastructure.MediatoR.Common.Decorators;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;


namespace FSTD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Add services
            AddServices(services, configuration);

            // Add configurations for OpenIddict and authentication
            AddConfigurations(services, configuration);

            return services;
        }

        // Method for adding general services such as DbContext, Identity, Repositories, and Application Services
        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            // Automatically register services
            services.AddAutoRegisteredServices(Assembly.GetExecutingAssembly());

            //MediatoR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestsDecorator<,>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            //Adding Autpmapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Adding Swagger generation with JWT Bearer configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FSTD API V1", Version = "v1" });

                // Configure JWT Bearer token authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Just enter Bearer then the token only, Example: Bearer accessToken",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
            });
        }

        // Method for adding OpenIddict configuration and Authentication/Authorization-related configurations
        private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            //Adding Identities
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            CustomIdentityOptions.ConfigureIdentityOptions(services);

            //Adding DbContext
            var connectionString = configuration.GetConnectionString("ApplicationDbContext");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    builder =>
                    {
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        builder.CommandTimeout(180);
                        builder.MigrationsAssembly("FSTD.DataCore");
                    });
                //options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();

            });

            //Adding JWT authentication and Validation.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var jwtTokenConfig = configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfigModel>();
            if (jwtTokenConfig == null)
            {
                throw new ArgumentNullException(nameof(jwtTokenConfig), "JWT configuration is missing");
            }
            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtTokenConfig.Issuer,
                        ValidAudience = jwtTokenConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.SecretKey)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.ConfigureApplicationCookie(config =>
            {
                config.SlidingExpiration = true;
                config.ExpireTimeSpan = TimeSpan.FromDays(1);// caching the Auth. for 1 day.
            });

            //Adding Autherization to all controller "MinimumAdminPortalRole".
            services.AddControllers();
        }

        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var seedService = serviceScope.ServiceProvider.GetRequiredService<IIdentitySeedService>();

                    // Ensure roles are seeded
                    seedService.SeedRoles().Wait();

                    // Seed users
                    seedService.SeedSuperUser().Wait();
                    seedService.SeedAdminUser().Wait();
                    seedService.SeedUser().Wait();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DatabaseSeeder");

                logger.LogError(ex, "An error occurred while seeding the database.");

                throw;
            }
        }

    }
}
