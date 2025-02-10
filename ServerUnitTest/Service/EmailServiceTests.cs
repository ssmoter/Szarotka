using MailKit.Net.Smtp;

using Microsoft.Extensions.Configuration;

using MimeKit;

using Moq;

using Server.Model;
using Server.Service;

namespace ServerUnitTest.Service
{
    public class EmailServiceTests
    {
        private readonly EmailService _emailService;
        private readonly EmailConfiguration _emailConfig;
        private readonly Mock<ISmtpClient> _smtpClientMock;

        public EmailServiceTests()
        {
            _emailConfig = new EmailConfiguration
            {
                From = "from@example.com",
                SmtpServer = "smtp.example.com",
                Port = 587,
                UserName = "username",
                Password = "password"
            };

            var inMemorySettings = new Dictionary<string, string?>
            {
                { "EmailConfiguration:From", _emailConfig.From },
                { "EmailConfiguration:SmtpServer", _emailConfig.SmtpServer },
                { "EmailConfiguration:Port", _emailConfig.Port.ToString() },
                { "EmailConfiguration:UserName", _emailConfig.UserName },
                { "EmailConfiguration:Password", _emailConfig.Password }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _smtpClientMock = new Mock<ISmtpClient>();
            _emailService = new EmailService(configuration, _smtpClientMock.Object);
        }

        [Fact]
        public void CreatedMessage_ShouldReturnMimeMessage()
        {
            // Arrange
            var to = "to@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            var message = _emailService.CreatedMessage(to, subject, body);

            // Assert
            Assert.NotNull(message);
            Assert.Equal(subject, message.Subject);
            Assert.Equal(body, message.HtmlBody);
            Assert.Single(message.To);
            Assert.Single(message.From);
        }

        [Fact]
        public async Task SendMessage_ShouldSendMimeMessage()
        {
            // Arrange
            var message = _emailService.CreatedMessage("to@example.com", "Test Subject", "Test Body");
            var cancellationToken = new CancellationToken();

            // Act & Assert
            await _emailService.SendMessage(message, cancellationToken);
        }

        [Fact]
        public async Task SendMessage_WithParameters_ShouldSendMimeMessage()
        {
            // Arrange
            var to = "to@example.com";
            var subject = "Test Subject";
            var body = "Test Body";
            var cancellationToken = new CancellationToken();

            // Act & Assert
            await _emailService.SendMessage(to, subject, body, cancellationToken);
        }

        [Fact]
        public async Task SendMessages_ShouldSendMultipleMimeMessages()
        {
            // Arrange
            var messages = new List<MimeMessage>
            {
                _emailService.CreatedMessage("to1@example.com", "Test Subject 1", "Test Body 1"),
                _emailService.CreatedMessage("to2@example.com", "Test Subject 2", "Test Body 2")
            };
            var cancellationToken = new CancellationToken();

            // Act & Assert
            await _emailService.SendMessages(messages, cancellationToken);
        }
    }
}
