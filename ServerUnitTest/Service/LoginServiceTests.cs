using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.AspNetCore.Http;

using Moq;

using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Service
{
    public class LoginServiceTests
    {
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IUserValidation> _mockUserValidation;
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockTimeService = new Mock<ITimeService>();
            _mockUserValidation = new Mock<IUserValidation>();
            _loginService = new LoginService(_mockDb.Object, _mockTimeService.Object, _mockUserValidation.Object);
        }

        [Fact]
        public async Task LogIn_ValidUser_ReturnsUser()
        {
            // Arrange
            var loginUser = new LoginUser { Email = "test@example.com", Password = "password", RememberMe = true };
            var user = new User { Id = Guid.NewGuid(), Email = loginUser.Email, RememberMe = false };

            _mockDb.Setup(db => db.DataBaseAsync.QueryAsync<User>(It.IsAny<string>())).ReturnsAsync(new List<User> { user });
            _mockUserValidation.Setup(v => v.AccountNotFound(It.IsAny<User>()));

            // Act
            var result = await _loginService.LogIn(loginUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            _mockUserValidation.Verify(v => v.AccountNotFound(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LogIn_UserNotFound_ThrowsException()
        {
            // Arrange
            var loginUser = new LoginUser { Email = "test@example.com", Password = "password" };

            _mockDb.Setup(db => db.DataBaseAsync.QueryAsync<User>(It.IsAny<string>())).ReturnsAsync(new List<User>());
            _mockUserValidation.Setup(v => v.AccountNotFound(It.IsAny<User>())).Throws(new ValidationException());

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _loginService.LogIn(loginUser));
        }

        [Fact]
        public async Task LogOut_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var loginUser = new LoginUser { Email = "test@example.com", Password = "password" };

            // Act
            var result = await _loginService.LogOut(loginUser);

            // Assert
            Assert.Equal(Results.Ok(), result);
        }
    }
}
