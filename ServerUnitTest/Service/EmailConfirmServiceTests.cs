using DataBase.Model.EntitiesServer;

using Moq;

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
            _mockEmailService = new Mock<IEmailService>();
            _mockRegisterUserService = new Mock<IRegisterUserService>();
            _emailConfirmService = new EmailConfirmService(_mockEmailService.Object, _mockRegisterUserService.Object);
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
