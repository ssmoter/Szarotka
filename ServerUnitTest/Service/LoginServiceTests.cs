using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.AspNetCore.Http;

using Moq;

using Server.Helper;
using Server.Service;
using Server.SqlQuery;
using Server.Validation;

namespace ServerUnitTest.Service
{
    public class LoginServiceTests
    {
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IUserValidation> _mockUserValidation;
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _mockTimeService = new Mock<ITimeService>();
            _mockUserValidation = new Mock<IUserValidation>();
            _loginService = new LoginService(_mockDb.Object, _mockTimeService.Object, _mockUserValidation.Object);
        }

        [Fact]
        public async Task LogIn_ValidUser_ReturnsUser()
        {
            // Arrange
            var loginUser = new LoginUser { Email = "test@example.com", Password = "password", RememberMe = true };
            var hashedPassword = Hash.PasswordSHA256(loginUser.Password);
            var user = new User { Id = Guid.NewGuid(), Email = loginUser.Email, RememberMe = false };

            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()))
                   .ReturnsAsync([user]);

            _mockUserValidation.Setup(v => v.AccountNotFound(It.IsAny<User>()));

            _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());

            _mockTimeService.Setup(t => t.UtcNow()).Returns(DateTime.UtcNow);

            // Act
            var result = await _loginService.LogIn(loginUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loginUser.Email, result.Email);
            _mockDb.Verify(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()), Times.Once);
            _mockUserValidation.Verify(v => v.AccountNotFound(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LogIn_UserNotFound_ThrowsException()
        {
            // Arrange
            var loginUser = new LoginUser { Email = "test@example.com", Password = "password" };
            var hashedPassword = Hash.PasswordSHA256(loginUser.Password);

            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()))
                   .ReturnsAsync([]);

            _mockUserValidation.Setup(v => v.AccountNotFound(null))
                               .Throws(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _loginService.LogIn(loginUser));
            _mockDb.Verify(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()), Times.Once);
            _mockUserValidation.Verify(v => v.AccountNotFound(null), Times.Once);
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

        [Fact]
        public async Task GetPublicUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = "test-id";
            var user = new User { Id = Guid.NewGuid(), Name = "Test User" };
            var users = new List<User> { user };

            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(users);

            // Act
            var result = await _loginService.GetPublicUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.Name);
        }

        [Fact]
        public async Task GetPublicUser_ShouldThrowArgumentNullException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "test-id";
            var users = new List<User>();

            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(users);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _loginService.GetPublicUser(userId));
        }
    }
}
