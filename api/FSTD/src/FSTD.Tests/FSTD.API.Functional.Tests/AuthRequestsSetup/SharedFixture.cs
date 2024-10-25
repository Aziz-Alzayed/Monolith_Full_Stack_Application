using Microsoft.AspNetCore.Mvc.Testing;

namespace FSTD.API.Functional.Tests.AuthRequestsSetup
{
    public class SharedFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        private readonly WebApplicationFactory<Program> _factory;

        public SharedFixture()
        {
            _factory = new WebApplicationFactory<Program>();
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
