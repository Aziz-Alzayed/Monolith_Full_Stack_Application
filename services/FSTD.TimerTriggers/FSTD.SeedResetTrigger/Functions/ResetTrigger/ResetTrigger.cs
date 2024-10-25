using FSTD.SeedResetTrigger.Functions.ResetTrigger.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FSTD.SeedResetTrigger.Functions.ResetTrigger
{
    public class ResetTrigger
    {
        private readonly ILogger<ResetTrigger> _logger;
        private readonly IRequestSender _requestSender;

        public ResetTrigger(ILogger<ResetTrigger> logger, IRequestSender requestSender)
        {
            _logger = logger;
            _requestSender = requestSender;
        }

        [Function(nameof(ResetTrigger))]
        public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Timer function executed at: {DateTime.Now}");

            try
            {
                // Send the seed reset request
                await _requestSender.SendSeedResetRequestAsync();
            }
            catch (HttpRequestException httpEx)
            {
                // Log specific details for HTTP request failures
                _logger.LogError(httpEx, $"HTTP request failed during the seed reset at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                // Log general exception
                _logger.LogError(ex, $"An unexpected error occurred during the seed reset at {DateTime.Now}");
            }
        }
    }
}
