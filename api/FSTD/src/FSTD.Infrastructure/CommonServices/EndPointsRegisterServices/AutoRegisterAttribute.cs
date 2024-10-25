using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.CommonServices.EndPointsRegisterServices
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AutoRegisterAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; }
        public bool ShouldRegister { get; }

        public AutoRegisterAttribute(ServiceLifetime lifetime, bool shouldRegister = true)
        {
            Lifetime = lifetime;
            ShouldRegister = shouldRegister;
        }

    }
}
