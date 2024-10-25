using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FSTD.SeedResetTrigger.Functions.ResetTrigger.Services
{
    public interface IRequestSender
    {
        Task SendSeedResetRequestAsync();
    }

    public class RequestSender : IRequestSender
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RequestSender> _logger;
        private const string ApiKeyHeaderName = "X-API-KEY";
        private const string SeedApiKeyConfigKey = "SeedApiKey";
        private const string SeedResetEndpointConfigKey = "SeedResetEndpoint";

        public RequestSender(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<RequestSender> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendSeedResetRequestAsync()
        {
            try
            {
                var apiKey = GetApiKey();
                if (string.IsNullOrEmpty(apiKey)) return;

                var endpoint = GetEndpoint();
                if (string.IsNullOrEmpty(endpoint)) return;

                var request = BuildHttpRequest(endpoint, apiKey);
                await SendRequestAsync(request);
            }
            catch
            {

                throw;
            }
        }

        private string GetApiKey()
        {
            var apiKey = _configuration[SeedApiKeyConfigKey];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("API key not found in configuration.");
            }
            return apiKey;
        }

        private string GetEndpoint()
        {
            var endpoint = _configuration[SeedResetEndpointConfigKey];
            if (string.IsNullOrEmpty(endpoint))
            {
                _logger.LogError("Seed reset endpoint not found in configuration.");
            }
            return endpoint;
        }

        private HttpRequestMessage BuildHttpRequest(string endpoint, string apiKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add(ApiKeyHeaderName, apiKey);
            return request;
        }

        private async Task SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Request succeeded at {DateTime.Now}, Status Code: {response.StatusCode}");
                }
                else
                {
                    _logger.LogError($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
