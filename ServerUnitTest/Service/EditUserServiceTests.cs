using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Moq;

using Server.Service;

namespace ServerUnitTest.Service
{
    public class EditUserServiceTests
    {
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly EditUserService _editUserService;

        public EditUserServiceTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockTimeService = new Mock<ITimeService>();
            _editUserService = new EditUserService(_mockDb.Object, _mockTimeService.Object);
        }

        [Fact]
        public async Task UpdateName_ShouldUpdateUserName()
        {
            // Arrange
            var user = new User { Name = "OldName" };
            _mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>())).ReturnsAsync(1);

            // Act
            await _editUserService.UpdateName(user);

            // Assert
            _mockTimeService.Verify(ts => ts.UtcNow(), Times.Once);
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDescription_ShouldUpdateUserDescription()
        {
            // Arrange
            var user = new User { Description = "OldDescription" };
            _mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>())).ReturnsAsync(1);

            // Act
            await _editUserService.UpdateDescription(user);

            // Assert
            _mockTimeService.Verify(ts => ts.UtcNow(), Times.Once);
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmail_ShouldUpdateUserEmail()
        {
            // Arrange
            var user = new User { Email = "old@example.com" };
            _mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>())).ReturnsAsync(1);

            // Act
            await _editUserService.UpdateEmail(user);

            // Assert
            _mockTimeService.Verify(ts => ts.UtcNow(), Times.Once);
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePhoneNumber_ShouldUpdateUserPhoneNumber()
        {
            // Arrange
            var user = new User { PhoneNumber = "1234567890" };
            _mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>())).ReturnsAsync(1);

            // Act
            await _editUserService.UpdatePhoneNumber(user);

            // Assert
            _mockTimeService.Verify(ts => ts.UtcNow(), Times.Once);
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserType_ShouldUpdateUserType()
        {
            // Arrange
            var user = new User { UserType = UserType.Driver };
            _mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            _mockDb.Setup(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<UserType>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>())).ReturnsAsync(1);

            // Act
            await _editUserService.UpdateUserType(user);

            // Assert
            _mockTimeService.Verify(ts => ts.UtcNow(), Times.Once);
            _mockDb.Verify(db => db.DataBaseAsync.ExecuteAsync(It.IsAny<string>()
                , It.IsAny<UserType>()
                , It.IsAny<long>()
                , It.IsAny<Guid>()
                , It.IsAny<Guid>()), Times.Once);
        }

    }
}
