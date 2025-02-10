using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Moq;

using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Service
{

    public class RegisterUserServiceTests
    {
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly IUserValidation _userValidation;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly RegisterUserService _service;

        public RegisterUserServiceTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockTimeService = new Mock<ITimeService>();
            _userValidation = new UserValidation(_mockDb.Object, _mockTimeService.Object);
            _service = new RegisterUserService(_mockDb.Object, _userValidation, _mockTimeService.Object);
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
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(1);

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
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            await _service.InsertCodeEmailAndRemoveOld(confirmCode);

            // Assert
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetUserEmailFromCodeAndRemoveOld_ShouldReturnUserEmail()
        {
            // Arrange
            var code = 1234;
            var confirmCode = new ConfirmCode { UserId = Guid.NewGuid(), Code = code, ExpireDate = DateTime.UtcNow.AddMinutes(10).Ticks };
            var user = new User { Id = confirmCode.UserId, IsEmailConfirm = true };
            _mockDb.Setup(db => db.DataBaseAsync.QueryAsync<ConfirmCode>(It.IsAny<string>())).ReturnsAsync(new List<ConfirmCode> { confirmCode });
            _mockDb.Setup(db => db.DataBaseAsync.QueryAsync<User>(It.IsAny<string>())).ReturnsAsync(new List<User> { user });
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(1);
            // Act
            var result = await _service.GetUserEmailFromCodeAndRemoveOld(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(confirmCode.UserId, result.Id);
            Assert.True(result.IsEmailConfirm);
        }

    }
}
