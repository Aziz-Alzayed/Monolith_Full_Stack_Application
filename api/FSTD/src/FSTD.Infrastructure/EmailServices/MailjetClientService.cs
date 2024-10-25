using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.EmailServices
{
    public interface IMailjetClientService
    {
        Task<bool> SendEmailAsync(string apiKey, string apiSecret, MailjetRequest request);
    }
    [AutoRegister(ServiceLifetime.Singleton)]
    public class MailjetClientService : IMailjetClientService
    {
        public async Task<bool> SendEmailAsync(string apiKey, string apiSecret, MailjetRequest request)
        {
            var client = new MailjetClient(apiKey, apiSecret);
            var response = await client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
