using DataBase.Model.EntitiesServer;

using Microsoft.Extensions.Configuration;

using Moq;

using Server.Model;
using Server.Service;

namespace ServerUnitTest.Service
{
    public class EmailConfirmServiceTests
    {
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IRegisterUserService> _mockRegisterUserService;
        private readonly EmailConfirmService _emailConfirmService;

        public EmailConfirmServiceTests()
        {
           var emailConfig = new EmailConfiguration
            {
                From = "from@example.com",
                SmtpServer = "smtp.example.com",
                Port = 587,
                UserName = "username",
                Password = "password"
            };

            var inMemorySettings = new Dictionary<string, string?>
            {
                { "EmailConfiguration:From", emailConfig.From },
                { "EmailConfiguration:SmtpServer", emailConfig.SmtpServer },
                { "EmailConfiguration:Port", emailConfig.Port.ToString() },
                { "EmailConfiguration:UserName", emailConfig.UserName },
                { "EmailConfiguration:Password", emailConfig.Password }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockEmailService = new Mock<IEmailService>();
            _mockRegisterUserService = new Mock<IRegisterUserService>();
            _emailConfirmService = new EmailConfirmService(_mockEmailService.Object, _mockRegisterUserService.Object,configuration);
        }

        [Fact]
        public async Task SendVerificationEmailCode_ShouldSendEmailAndSaveCode()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com"
            };

            _mockRegisterUserService
                .Setup(s => s.InsertCodeEmailAndRemoveOld(It.IsAny<ConfirmCode>()))
                .Returns(Task.CompletedTask);

            _mockEmailService
                .Setup(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _emailConfirmService.SendVerificationEmailCode(user);

            // Assert
            _mockRegisterUserService.Verify(s => s.InsertCodeEmailAndRemoveOld(It.Is<ConfirmCode>(c => c.UserId == user.Id)), Times.Once);
            _mockEmailService.Verify(s => s.SendMessage(user.Email, "Potwierdź swój email", It.IsAny<string>(), default), Times.Once);
        }
    }
}
