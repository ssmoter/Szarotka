using DataBase.Data;
using DataBase.Service;

using Server.Model;
using Server.Validation;

namespace ServerUnitTest.Validation.User
{
    public class EditUserTest
    {

        private readonly IUserValidation _userValidation;

        public EditUserTest()
        {
            _userValidation = new UserValidation(new AccessDataBase(), new CurrentUtc());
        }

        [Theory]
        [InlineData("name")]
        public void NameRequiredTestValid(string name)
        {
            var result = _userValidation.NameRequired(name);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void NameRequiredTestInvalid(string name)
        {
            var result = _userValidation.NameRequired(name);
            Assert.Equal(ServerEnums.Result.Error, result);
        }

        [Theory]
        [InlineData("123456789")]
        [InlineData("1234")]
        public void PhoneNumberRequiredTestValid(string phoneNumber)
        {
            var result = _userValidation.PhoneNumberRequired(phoneNumber);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void PhoneNumberRequiredTestInvalid(string phoneNumber)
        {
            var result = _userValidation.PhoneNumberRequired(phoneNumber);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Theory]
        [InlineData("123456789")]
        public void PhoneNumberPatternTestValid(string phoneNumber)
        {
            var result = _userValidation.PhoneNumberPattern(phoneNumber);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Theory]
        [InlineData("number")]
        [InlineData("4535h3432")]
        public void PhoneNumberPatternTestInvalid(string phoneNumber)
        {
            var result = _userValidation.PhoneNumberPattern(phoneNumber);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
    }
}
