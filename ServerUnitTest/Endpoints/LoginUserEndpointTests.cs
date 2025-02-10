using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Endpoints;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Endpoints
{
    public class LoginUserEndpointTests
    {
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly Mock<IEmailConfirmService> _mockEmailConfirmService;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly IUserValidation _userValidation;
        private readonly LoginUserEndpoint _loginUserEndpoint;

        public LoginUserEndpointTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockLoginService = new Mock<ILoginService>();
            _mockEmailConfirmService = new Mock<IEmailConfirmService>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            var _timeService = new Mock<ITimeService>();
            _userValidation = new UserValidation(_mockDb.Object, _timeService.Object, new ValidationException());

            _loginUserEndpoint = new LoginUserEndpoint(
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
            var result = await _loginUserEndpoint.LogInUser(user);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task LogOutUser_ValidUser_ReturnsOkResult()
        {
            // Act
            var result = await _loginUserEndpoint.LogOutUser("testUser");

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
            var result = await _loginUserEndpoint.RefreshToken(token);

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
            var result = await _loginUserEndpoint.RefreshToken(token);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }
    }
}
