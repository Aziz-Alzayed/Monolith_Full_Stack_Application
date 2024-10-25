using FSTD.Exeptions.Models.ServicesExeptions;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace FSTD.Infrastructure.EmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
    [AutoRegister(ServiceLifetime.Singleton)]
    public class EmailService : IEmailService
    {
        private readonly IMailjetClientService _mailjetClientService;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _fromEmail;
        private readonly string _companyName;

        public EmailService(IConfiguration configuration, IMailjetClientService mailjetClientService)
        {
            _apiKey = configuration["Mailjet:ApiKey"] ?? "";
            _apiSecret = configuration["Mailjet:ApiSecret"] ?? "";
            _fromEmail = configuration["Mailjet:FromEmail"] ?? "";
            _companyName = configuration["Mailjet:CompanyName"] ?? "";
            _mailjetClientService = mailjetClientService;
        }
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, _fromEmail)
                .Property(Send.FromName, _companyName)
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, body)
                .Property(Send.Recipients, new JArray {
                new JObject {
                    {"Email", toEmail}
                }
                });

                return await _mailjetClientService.SendEmailAsync(_apiKey, _apiSecret, request);
            }
            catch (Exception ex)
            {
                throw new EmailServiceException(ex.Message, ex);
            }
        }
    }
}
