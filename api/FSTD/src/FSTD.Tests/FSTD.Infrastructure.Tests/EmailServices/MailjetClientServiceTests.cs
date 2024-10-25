using FSTD.Infrastructure.EmailServices;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Moq;
using Newtonsoft.Json.Linq;

namespace FSTD.Infrastructure.Unit.Tests.EmailServices
{
    public class MailjetClientServiceTests
    {
        private readonly Mock<IMailjetClient> _mailjetClientMock;
        private readonly IMailjetClientService _mailjetClientService;

        public MailjetClientServiceTests()
        {
            // Mocking MailjetClient
            _mailjetClientMock = new Mock<IMailjetClient>();

            // Instantiate the MailjetClientService (with the mock MailjetClient)
            _mailjetClientService = new MailjetClientService();
        }

        [Fact]
        public async Task SendEmailAsync_Should_Return_False_When_Unauthorized()
        {
            // Arrange
            var apiKey = "test-api-key";
            var apiSecret = "test-api-secret";
            var request = new MailjetRequest { Resource = Send.Resource };

            // Mock successful response
            var responseContent = new JObject(); // Simulating an empty content response for successful email
            var response = new MailjetResponse(false, 401, responseContent); // Pass all required parameters

            _mailjetClientMock.Setup(client => client.PostAsync(It.IsAny<MailjetRequest>()))
                              .ReturnsAsync(response);

            // Act
            var result = await _mailjetClientService.SendEmailAsync(apiKey, apiSecret, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Return_False_When_Email_Fails_To_Send()
        {
            // Arrange
            var apiKey = "invalid-api-key";
            var apiSecret = "invalid-api-secret";
            var request = new MailjetRequest { Resource = Send.Resource };

            // Mock failed response
            var responseContent = new JObject(); // Simulating an empty content response for failure
            var response = new MailjetResponse(false, 401, responseContent); // Unauthorized error code

            _mailjetClientMock.Setup(client => client.PostAsync(It.IsAny<MailjetRequest>()))
                              .ReturnsAsync(response);

            // Act
            var result = await _mailjetClientService.SendEmailAsync(apiKey, apiSecret, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Handle_Exception_Gracefully()
        {
            // Arrange
            var apiKey = "test-api-key";
            var apiSecret = "test-api-secret";
            var request = new MailjetRequest { Resource = Send.Resource };

            // Mock exception thrown
            _mailjetClientMock.Setup(client => client.PostAsync(It.IsAny<MailjetRequest>()))
                              .ThrowsAsync(new System.Net.Http.HttpRequestException("Network error"));

            // Act
            var result = await _mailjetClientService.SendEmailAsync(apiKey, apiSecret, request);

            // Assert: Should return false in case of an exception
            Assert.False(result);
        }
    }
}
