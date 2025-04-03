using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Http;

using Moq;

using Server.Model;
using Server.Requests;
using Server.Service;
using Server.Validation;

namespace ServerUnitTest.Requests;
public class ResetPasswordRequestsTests
{
    private readonly Mock<IAccessDataBase> _mockDb;
    private readonly Mock<IUserValidation> _mockUserValidation;
    private readonly Mock<IEmailConfirmService> _mockEmailConfirmService;
    private readonly Mock<IResetPasswordService> _mockResetPasswordService;
    private readonly ResetPasswordRequests _resetPasswordRequests;

    public ResetPasswordRequestsTests()
    {
        _mockDb = new Mock<IAccessDataBase>();
        _mockUserValidation = new Mock<IUserValidation>();
        _mockEmailConfirmService = new Mock<IEmailConfirmService>();
        _mockResetPasswordService = new Mock<IResetPasswordService>();

        _resetPasswordRequests = new ResetPasswordRequests(
            _mockDb.Object,
            _mockUserValidation.Object,
            _mockResetPasswordService.Object,
            _mockEmailConfirmService.Object
        );
    }

    [Fact]
    public async Task ResetPasswordEmail_ValidEmail_ReturnsOk()
    {
        // Arrange
        var email = "test@example.com";
        var userId = Guid.NewGuid();
        _mockUserValidation.Setup(v => v.EmailIsNull(email)).Returns(ServerEnums.Result.Success);
        _mockResetPasswordService.Setup(s => s.GetUserIdFromEmail(email)).ReturnsAsync(new User { Id = userId });
        _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());

        // Act
        var result = await _resetPasswordRequests.ResetPasswordEmail(email);

        // Assert
        Assert.Equal(Results.Ok().GetType(), result.GetType());
        _mockEmailConfirmService.Verify(s => s.SendResetPasswordEmailCode(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordCode_ValidCode_ReturnsOk()
    {
        // Arrange
        var code = 123456;
        var confirmCode = new ConfirmCode { Code = code };
        _mockResetPasswordService.Setup(s => s.GetConfirmCode(code)).ReturnsAsync(confirmCode);
        _mockUserValidation.Setup(v => v.CodeNotExist(confirmCode)).Returns(ServerEnums.Result.Success);
        _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());

        // Act
        var result = await _resetPasswordRequests.ResetPasswordCode(code);

        // Assert
        Assert.Equal(Results.Ok().GetType(), result.GetType());
    }

    [Fact]
    public async Task ResetPasswordNew_ValidCodeAndPassword_ReturnsOk()
    {
        // Arrange
        var code = 123456;
        var password = "NewPassword123!";
        var confirmCode = new ConfirmCode { Code = code, UserId = Guid.NewGuid() };
        _mockResetPasswordService.Setup(s => s.GetConfirmCode(code)).ReturnsAsync(confirmCode);
        _mockUserValidation.Setup(v => v.CodeNotExist(confirmCode)).Returns(ServerEnums.Result.Success);
        _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());

        // Act
        var result = await _resetPasswordRequests.ResetPasswordNew(code, password);

        // Assert
        Assert.Equal(Results.Ok().GetType(), result.GetType());
        _mockResetPasswordService.Verify(s => s.ChangePassword(confirmCode.UserId, password), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordEmail_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var email = "invalid-email";
        string message = $"{nameof(RegisterUser.Email)} is null";
        var error = new List<ValidationException.Valid>
        {
            new(message, EnumsList.Validation.EmailIsNull)
        };
        var errors = new ValidationException();
        errors.ValidationErrors.Add(error[0]);

        _mockUserValidation.Setup(v => v.EmailIsNull(email)).Returns(ServerEnums.Result.Error);
        _mockUserValidation.Setup(v => v.Validation.GetError()).Returns(message);
        _mockUserValidation.Setup(x => x.Validation).Returns(errors);
        _mockUserValidation.Setup(x => x.Validation.Throw()).Throws(errors);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _resetPasswordRequests.ResetPasswordEmail(email));
    }

    [Fact]
    public async Task ResetPasswordCode_InvalidCode_ThrowsValidationException()
    {
        // Arrange
        var code = 123456;
        string message = $"Not find a selected {nameof(ConfirmCode.Code)}";
        var error = new List<ValidationException.Valid>
        {
            new(message, EnumsList.Validation.CodeNotExist)
        };
        var errors = new ValidationException();
        errors.ValidationErrors.Add(error[0]);

        _mockResetPasswordService.Setup(s => s.GetConfirmCode(code)).ReturnsAsync((ConfirmCode)null!);
        _mockUserValidation.Setup(v => v.CodeNotExist(null)).Returns(ServerEnums.Result.Error);
        _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());
        _mockUserValidation.Setup(v => v.Validation.GetError()).Returns("Invalid code");
        _mockUserValidation.Setup(v => v.Validation.ValidationErrors).Returns(error);
        _mockUserValidation.Setup(x => x.Validation.Throw()).Throws(errors);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _resetPasswordRequests.ResetPasswordCode(code));
    }

    [Fact]
    public async Task ResetPasswordNew_InvalidCode_ThrowsValidationException()
    {
        // Arrange
        var code = 123456;
        var password = "NewPassword123!";
        string message = $"Not find a selected {nameof(ConfirmCode.Code)}";
        var error = new List<ValidationException.Valid>
        {
            new(message, EnumsList.Validation.CodeNotExist)
        };
        var errors = new ValidationException();
        errors.ValidationErrors.Add(error[0]);

        _mockUserValidation.Setup(x => x.Validation).Returns(new ValidationException());
        _mockUserValidation.Setup(x => x.Validation.ValidationErrors).Returns([]);
        _mockResetPasswordService.Setup(s => s.GetConfirmCode(code)).ReturnsAsync((ConfirmCode)null!);
        _mockUserValidation.Setup(v => v.CodeNotExist(null)).Returns(ServerEnums.Result.Error);
        _mockUserValidation.Setup(v => v.Validation.ValidationErrors).Returns(error);
        _mockUserValidation.Setup(x => x.Validation.Throw()).Throws(errors);
        _mockUserValidation.Setup(x => x.Validation.Throw()).Throws(errors);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _resetPasswordRequests.ResetPasswordNew(code, password));
    }
}
