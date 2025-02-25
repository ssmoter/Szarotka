using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Server.Requests;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Requests
{
    public class LoginUserRequestsTests
    {
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly Mock<IEmailConfirmService> _mockEmailConfirmService;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly IUserValidation _userValidation;
        private readonly LoginUserRequests _loginUserRequests;

        public LoginUserRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockLoginService = new Mock<ILoginService>();
            _mockEmailConfirmService = new Mock<IEmailConfirmService>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            var _timeService = new Mock<ITimeService>();
            _userValidation = new UserValidation(_mockDb.Object, _timeService.Object, new ValidationException());

            _loginUserRequests = new LoginUserRequests(
                _mockDb.Object,
                _mockLoginService.Object,
                _mockEmailConfirmService.Object,
                _mockAuthenticationService.Object,
                _userValidation
            );
        }

        [Fact]
        public async Task LogInUser_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var user = new LoginUser { Email = "test@example.com", Password = "password" };
            var dbUser = new User { Email = "test@example.com", IsEmailConfirm = true, IsDelete = false };
            var token = new User { Token = "token" };
            _mockLoginService.Setup(s => s.LogIn(user)).ReturnsAsync(dbUser);
            _mockAuthenticationService.Setup(s => s.AuthenticateAsync(dbUser)).ReturnsAsync(token);

            // Act
            var result = await _loginUserRequests.LogInUser(user);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task LogOutUser_ValidUser_ReturnsOkResult()
        {
            // Act
            var result = await _loginUserRequests.LogOutUser("testUser");

            // Assert
            Assert.IsType<Ok>(result);
        }

        [Fact]
        public async Task RefreshToken_ValidToken_ReturnsOkResult()
        {
            // Arrange
            var token = "validToken";
            var newToken = new User { Token = "newToken" };

            _mockAuthenticationService.Setup(s => s.AuthenticateAsync(token)).ReturnsAsync(newToken);

            // Act
            var result = await _loginUserRequests.RefreshToken(token);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task RefreshToken_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            var token = "invalidToken";

            _mockAuthenticationService.Setup(s => s.AuthenticateAsync(token)).ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _loginUserRequests.RefreshToken(token);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        [Fact]
        public async Task GetPublicUser_ShouldReturnOkResult_WhenUserExists()
        {
            // Arrange
            var userId = Guid.CreateVersion7().ToString();
            var user = new User { Id = new Guid(userId), Name = "Test User" };
            _mockLoginService.Setup(service => service.GetPublicUser(userId)).ReturnsAsync(user);

            // Act
            var result = await _loginUserRequests.GetPublicUser(userId);

            // Assert
            var okResult = Assert.IsType<Ok<User>>(result);
            Assert.Equal(user.Name, okResult?.Value?.Name);
        }

        [Fact]
        public async Task GetPublicUser_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
        {
            // Arrange
            var userId = Guid.CreateVersion7().ToString();
            _mockLoginService.Setup(service => service.GetPublicUser(userId)).ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _loginUserRequests.GetPublicUser(userId);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        [Fact]
        public async Task GetPublicUser_ShouldThrowOperationCanceledException_WhenOperationIsCancelled()
        {
            // Arrange
            var userId = "test-id";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => _loginUserRequests.GetPublicUser(userId, cancellationTokenSource.Token));
        }

        [Fact]
        public async Task GetPublicUser_ShouldLogException_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = "test-id";
            var exception = new Exception("Test exception");
            _mockLoginService.Setup(service => service.GetPublicUser(userId)).ThrowsAsync(exception);

            // Act
            await Assert.ThrowsAsync<Exception>(() => _loginUserRequests.GetPublicUser(userId));

            // Assert
            _mockDb.Verify(db => db.SaveLog(exception), Times.Once);
        }

    }
}
