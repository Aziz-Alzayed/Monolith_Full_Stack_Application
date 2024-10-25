using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FSTD.Infrastructure.CommonServices.EndPointsRegisterServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoRegisteredServices(this IServiceCollection services, Assembly assembly)
        {
            var typesWithAttribute = assembly.GetTypes()
                .Where(type => type.GetCustomAttribute<AutoRegisterAttribute>() != null)
                .ToList();

            foreach (var type in typesWithAttribute)
            {
                var attribute = type.GetCustomAttribute<AutoRegisterAttribute>();

                // Check if ShouldRegister is false, and skip registration if so
                if (!attribute.ShouldRegister)
                {
                    Console.WriteLine($"Skipping registration for {type.Name} because ShouldRegister is set to false.");
                    continue;
                }

                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}");

                if (interfaceType != null)
                {
                    services.Add(new ServiceDescriptor(interfaceType, type, attribute.Lifetime));
                    Console.WriteLine($"Registering {interfaceType.Name} with {type.Name}");
                }
                else
                {
                    Console.WriteLine($"No matching interface found for {type.Name}");
                }
            }

            return services;
        }
    }
}
