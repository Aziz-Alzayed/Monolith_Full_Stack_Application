using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.Unit.Tests.CommonServices
{
    namespace FSTD.Infrastructure.CommonServices.EndPointsRegisterServices.Tests
    {
        public class ServiceCollectionExtensionsTests
        {
            private readonly IServiceCollection _services;

            public ServiceCollectionExtensionsTests()
            {
                _services = new ServiceCollection();
            }

            [Fact]
            public void Should_Register_Service_With_AutoRegisterAttribute()
            {
                // Arrange
                var assembly = typeof(MyService).Assembly;

                // Act
                _services.AddAutoRegisteredServices(assembly);
                var provider = _services.BuildServiceProvider();
                var service = provider.GetService<IMyService>();

                // Assert
                Assert.NotNull(service);
                Assert.IsType<MyService>(service);
            }

            [Fact]
            public void Should_Not_Register_Service_When_ShouldRegister_Is_False()
            {
                // Arrange
                var assembly = typeof(ServiceWithNoRegistration).Assembly;

                // Act
                _services.AddAutoRegisteredServices(assembly);
                var provider = _services.BuildServiceProvider();
                var service = provider.GetService<IServiceWithNoRegistration>();

                // Assert
                Assert.Null(service);
            }

            [Fact]
            public void Should_Not_Register_Service_Without_Matching_Interface()
            {
                // Arrange
                var assembly = typeof(ServiceWithoutInterface).Assembly;

                // Act
                _services.AddAutoRegisteredServices(assembly);
                var provider = _services.BuildServiceProvider();
                var service = provider.GetService<ServiceWithoutInterface>();

                // Assert
                Assert.Null(service);
            }
        }

        // Mock services for testing
        [AutoRegister(ServiceLifetime.Transient)]
        public class MyService : IMyService { }
        public interface IMyService { }

        [AutoRegister(ServiceLifetime.Singleton, shouldRegister: false)]
        public class ServiceWithNoRegistration : IServiceWithNoRegistration { }
        public interface IServiceWithNoRegistration { }

        [AutoRegister(ServiceLifetime.Scoped)]
        public class ServiceWithoutInterface { }
    }
}
