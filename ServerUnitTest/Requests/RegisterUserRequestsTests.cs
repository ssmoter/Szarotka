using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.AspNetCore.Http;

using Moq;

using Server.Model;
using Server.Requests;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Requests
{
    public class RegisterUserRequestsTests
    {
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly Mock<IRegisterUserService> _mockRegisterService;
        private readonly Mock<IUserValidation> _mockUserValidation;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IEmailConfirmService> _mockEmailConfirmService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly RegisterUserRequests _endpoint;

        public RegisterUserRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _mockRegisterService = new Mock<IRegisterUserService>();
            _mockUserValidation = new Mock<IUserValidation>();
            _mockEmailService = new Mock<IEmailService>();
            _mockEmailConfirmService = new Mock<IEmailConfirmService>();
            _mockTimeService = new Mock<ITimeService>();

            _endpoint = new RegisterUserRequests(
                _mockDb.Object,
                _mockRegisterService.Object,
                _mockUserValidation.Object,
                _mockEmailService.Object,
                _mockEmailConfirmService.Object,
                _mockTimeService.Object
            );
        }

        [Fact]
        public async Task InsertUser_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var registerUser = new RegisterUser { Email = "test@example.com", Password = "Password123!" };

            _mockUserValidation.Setup(v => v.Validation).Returns(new ValidationException());
            _mockUserValidation.Setup(v => v.RegisterUserNull(registerUser)).Returns(ServerEnums.Result.Success);
            _mockUserValidation.Setup(v => v.EmailIsNull(registerUser.Email)).Returns(ServerEnums.Result.Success);
            _mockUserValidation.Setup(v => v.PasswordIsNull(registerUser.Password)).Returns(ServerEnums.Result.Success);
            _mockRegisterService.Setup(s => s.InsertNewUser(registerUser)).ReturnsAsync(registerUser);

            // Act
            var result = await _endpoint.InsertUser(registerUser);

            // Assert
            Assert.Equal(Results.Ok().GetType(), result.GetType());
        }

        [Fact]
        public async Task ConfirmEmail_ShouldReturnOk_WhenCodeIsValid()
        {
            // Arrange
            var code = 123456;
            var user = new User { Email = "test@example.com" };
            _mockRegisterService.Setup(s => s.GetUserEmailFromCodeAndRemoveOld(code)).ReturnsAsync(user);

            // Act
            var result = await _endpoint.ConfirmEmail(code);

            // Assert
            Assert.Equal(Results.Ok(user).GetType(), result.GetType());
        }
    }
}
