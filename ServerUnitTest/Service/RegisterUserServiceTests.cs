using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.Extensions.Configuration;

using Moq;

using Server.Model;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Service
{

    public class RegisterUserServiceTests
    {
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly IUserValidation _userValidation;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly RegisterUserService _service;

        public RegisterUserServiceTests()
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
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _mockTimeService = new Mock<ITimeService>();
            _userValidation = new UserValidation(_mockDb.Object, _mockTimeService.Object);
            _service = new RegisterUserService(_mockDb.Object, _userValidation, _mockTimeService.Object, configuration);
        }

        [Fact]
        public async Task InsertNewUser_ShouldInsertUserSuccessfully()
        {
            // Arrange
            var registerUser = new RegisterUser
            {
                Id = Guid.Empty,
                Password = "password",
                Created = DateTime.MinValue
            };
            _mockTimeService.Setup(t => t.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(1);

            // Act
            var result = await _service.InsertNewUser(registerUser);

            // Assert
            Assert.False(result.IsDelete);
            Assert.False(result.IsEmailConfirm);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.NotEqual(DateTime.MinValue, result.Created);
            Assert.Equal(result.Created, result.Updated);
            Assert.NotEqual("password", result.Password); // Password should be hashed
        }

        [Fact]
        public async Task InsertCodeEmailAndRemoveOld_ShouldInsertAndRemoveOldCodes()
        {
            // Arrange
            var confirmCode = new ConfirmCode { UserId = Guid.NewGuid(), Code = 1234 };
            _mockTimeService.Setup(t => t.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(1);
            _mockDb.Setup(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(1);

            // Act
            await _service.InsertCodeEmailAndRemoveOld(confirmCode);

            // Assert
            _mockDb.Verify(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetUserEmailFromCodeAndRemoveOld_ShouldReturnUserEmail()
        {
            // Arrange
            var code = 1234;
            var confirmCode = new ConfirmCode { UserId = Guid.NewGuid(), Code = code, ExpireDate = DateTime.UtcNow.AddMinutes(10).Ticks };
            var user = new User { Id = confirmCode.UserId, IsEmailConfirm = true };

            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<ConfirmCode>(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync([confirmCode]);
            _mockDb.Setup(db => db.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>()
                , It.IsAny<Dictionary<string, object?>>())).ReturnsAsync([user]);
            _mockDb.Setup(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(1);
            _mockDb.Setup(db => db.DbAsyncAoT.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<Dictionary<string, object?>>())).ReturnsAsync(1);
            // Act
            var result = await _service.GetUserEmailFromCodeAndRemoveOld(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(confirmCode.UserId, result.Id);
            Assert.True(result.IsEmailConfirm);
        }

    }
}
