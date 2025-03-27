using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Moq;

using Server.Helper;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Service
{
    public class ResetPasswordServiceTests
    {
        private readonly Mock<IAccessDataBase> _dbMock;
        private readonly Mock<IUserValidation> _userValidationMock;
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly ResetPasswordService _resetPasswordService;

        public ResetPasswordServiceTests()
        {
            _dbMock = new Mock<IAccessDataBase>();
            _userValidationMock = new Mock<IUserValidation>();
            _timeServiceMock = new Mock<ITimeService>();
            _resetPasswordService = new ResetPasswordService(_dbMock.Object, _userValidationMock.Object, _timeServiceMock.Object);
        }

        [Fact]
        public async Task GetUserIdFromEmail_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };
            _dbMock.Setup(db => db.DataBaseAsync.QueryAsync<User>(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync([user]);

            _userValidationMock.Setup(x => x.Validation).Returns(new ValidationException());

            // Act
            var result = await _resetPasswordService.GetUserIdFromEmail(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetUserIdFromEmail_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            _dbMock.Setup(db => db.DataBaseAsync.QueryAsync<User>(It.IsAny<string>(), It.IsAny<object[]>()))
                   .ReturnsAsync([]);

            _userValidationMock.Setup(v => v.AccountNotFound(It.IsAny<User>())).Callback(() => throw new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _resetPasswordService.GetUserIdFromEmail(email));
        }

        [Fact]
        public async Task GetConfirmCode_ShouldReturnConfirmCode_WhenCodeExists()
        {
            // Arrange
            var code = 123456;
            var confirmCode = new ConfirmCode { Code = code };
            _dbMock.Setup(db => db.DataBaseAsync.QueryAsync<ConfirmCode>(It.IsAny<string>(), It.IsAny<object[]>()))
                   .ReturnsAsync([confirmCode]);

            // Act
            var result = await _resetPasswordService.GetConfirmCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(code, result.Code);
        }

        [Fact]
        public async Task GetConfirmCode_ShouldReturnNull_WhenCodeDoesNotExist()
        {
            // Arrange
            var code = 123456;
            _dbMock.Setup(db => db.DataBaseAsync.QueryAsync<ConfirmCode>(It.IsAny<string>(), It.IsAny<object[]>()))
                   .ReturnsAsync([]);

            // Act
            var result = await _resetPasswordService.GetConfirmCode(code);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ChangePassword_ShouldUpdatePassword()
        {
            // Arrange
            var id = Guid.NewGuid();
            var password = "newPassword";
            var hashedPassword = Hash.PasswordSHA256(password);
            var updateTicks = 123456789L;

            _timeServiceMock.Setup(t => t.UtcNow()).Returns(new DateTime(updateTicks));
            _dbMock.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>(), It.IsAny<object[]>()))
                   .Returns(Task.FromResult(1));

            // Act
            await _resetPasswordService.ChangePassword(id, password);

            // Assert
            _dbMock.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>(), It.Is<object[]>(args =>
                args.Contains(hashedPassword) && args.Contains(updateTicks) && args.Contains(id) && args.Contains(id))), Times.Once);
        }
    }
}
