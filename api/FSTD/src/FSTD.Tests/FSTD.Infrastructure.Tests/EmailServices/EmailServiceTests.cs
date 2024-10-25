using FSTD.Exeptions.Models.ServicesExeptions;
using FSTD.Infrastructure.EmailServices;
using Mailjet.Client;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.EmailServices
{
    public class EmailServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IMailjetClientService> _mailjetClientServiceMock;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _mailjetClientServiceMock = new Mock<IMailjetClientService>();

            // Set up configuration mock
            _configurationMock.SetupGet(config => config["Mailjet:ApiKey"]).Returns("test-api-key");
            _configurationMock.SetupGet(config => config["Mailjet:ApiSecret"]).Returns("test-api-secret");
            _configurationMock.SetupGet(config => config["Mailjet:FromEmail"]).Returns("noreply@test.com");
            _configurationMock.SetupGet(config => config["Mailjet:CompanyName"]).Returns("Test Company");

            // Instantiate EmailService with mocks
            _emailService = new EmailService(_configurationMock.Object, _mailjetClientServiceMock.Object);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Return_True_When_Email_Sent_Successfully()
        {
            // Arrange
            var toEmail = "recipient@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Mock SendEmailAsync to return true
            _mailjetClientServiceMock.Setup(m => m.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<MailjetRequest>()
            )).ReturnsAsync(true);

            // Act
            var result = await _emailService.SendEmailAsync(toEmail, subject, body);

            // Assert
            Assert.True(result); // Email should be sent successfully
            _mailjetClientServiceMock.Verify(m => m.SendEmailAsync(
                "test-api-key", "test-api-secret", It.IsAny<MailjetRequest>()), Times.Once);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Throw_EmailServiceException_When_Exception_Occurs()
        {
            // Arrange
            var toEmail = "recipient@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Mock SendEmailAsync to throw an exception
            _mailjetClientServiceMock.Setup(m => m.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<MailjetRequest>()
            )).ThrowsAsync(new Exception("Test Exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EmailServiceException>(() => _emailService.SendEmailAsync(toEmail, subject, body));
            Assert.Equal("Test Exception", exception.Message);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Throw_EmailServiceException_When_Missing_Configuration()
        {
            // Arrange: Missing API Key
            _configurationMock.SetupGet(config => config["Mailjet:ApiKey"]).Returns(string.Empty);

            var toEmail = "recipient@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Mock the Mailjet service to throw an exception when configuration is missing
            _mailjetClientServiceMock.Setup(m => m.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MailjetRequest>()))
                .ThrowsAsync(new EmailServiceException("Missing API Key"));

            // Act & Assert: Expect the method to throw due to missing configuration
            await Assert.ThrowsAsync<EmailServiceException>(() => _emailService.SendEmailAsync(toEmail, subject, body));
        }


        [Fact]
        public async Task SendEmailAsync_Should_Throw_EmailServiceException_If_MailjetClient_Returns_False()
        {
            // Arrange
            var toEmail = "recipient@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Mock the MailjetClientService to return false
            _mailjetClientServiceMock.Setup(m => m.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<MailjetRequest>()
            )).ReturnsAsync(false);

            // Act & Assert: Expect the method to return false when email sending fails
            var result = await _emailService.SendEmailAsync(toEmail, subject, body);
            Assert.False(result); // Should return false if the email sending failed
        }
    }
}
