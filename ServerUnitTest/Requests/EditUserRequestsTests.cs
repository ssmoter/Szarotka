using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Endpoints;
using Server.Requests;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Requests
{
    public class EditUserRequestsTests
    {
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly Mock<IUserValidation> _userValidation;
        private readonly Mock<IEditUserService> _mockEditUserService;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly EditUserRequests _editUserEndpoint;
        public EditUserRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _userValidation = new Mock<IUserValidation>();
            _mockEditUserService = new Mock<IEditUserService>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _editUserEndpoint = new EditUserRequests(
                _mockDb.Object,
                _userValidation.Object,
                _mockEditUserService.Object,
                _mockAuthenticationService.Object
            );
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            _mockAuthenticationService.Setup(a => a.AuthenticateAsync(It.IsAny<User>())).ReturnsAsync(new User());
            CreatedNewTokenSetup(editUser.New);

            // Act
            var result = await _editUserEndpoint.Update(editUser,It.IsAny<HttpContext>());

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task Update_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.Update(editUser,It.IsAny<HttpContext>()));
        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            CreatedNewTokenSetup(editUser.New);

            // Act
            var result = await _editUserEndpoint.UpdateDescription(editUser);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task UpdateDescription_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.UpdateDescription(editUser));
        }

        // Repeat similar tests for UpdateName, UpdateEmail, UpdatePhoneNumber, and UpdateUserType methods

        [Fact]
        public async Task UpdateName_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            CreatedNewTokenSetup(editUser.New);
            // Act
            var result = await _editUserEndpoint.UpdateName(editUser);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task UpdateName_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.UpdateName(editUser));
        }

        [Fact]
        public async Task UpdateEmail_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            CreatedNewTokenSetup(editUser.New);
            // Act
            var result = await _editUserEndpoint.UpdateEmail(editUser);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task UpdateEmail_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.UpdateEmail(editUser));
        }

        [Fact]
        public async Task UpdatePhoneNumber_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            CreatedNewTokenSetup(editUser.New);

            // Act
            var result = await _editUserEndpoint.UpdatePhoneNumber(editUser);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task UpdatePhoneNumber_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.UpdatePhoneNumber(editUser));
        }

        [Fact]
        public async Task UpdateUserType_ShouldReturnOk_WhenValidationPasses()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(0);
            _mockEditUserService.Setup(e => e.UpdateUserType(It.IsAny<User>()));
            CreatedNewTokenSetup(editUser.New);
            // Act
            var result = await _editUserEndpoint.UpdateUserType(editUser);

            // Assert
            Assert.IsType<Ok<User>>(result);
        }

        [Fact]
        public async Task UpdateUserType_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var editUser = new EditUser { New = new User(), Old = new User() };
            _userValidation.Setup(v => v.Validation.ValidationErrors.Count).Returns(1);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _editUserEndpoint.UpdateUserType(editUser));
        }



        private void CreatedNewTokenSetup(User user)
        {
            _mockAuthenticationService.Setup(_mockAuthenticationService => _mockAuthenticationService.AuthenticateAsync(It.IsAny<User>())).ReturnsAsync(new User());
            _mockDb.Setup(_mockDb => _mockDb.DbAsyncAoT.QueryAsync<User>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync([user]);
        }
    }
}
