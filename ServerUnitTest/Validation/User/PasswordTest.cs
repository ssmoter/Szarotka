using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.Validation;
using DataBase.Data.MySqliteConnection;

namespace ServerUnitTest.Validation.User
{
    public class PasswordTest
    {
        const string _validPassword = "Password1!";

        private readonly IUserValidation _userValidation;

        public PasswordTest()
        {
            var faktory = new SqliteConnectionFactory();
            var sync = new MyDbConnection(faktory);
            var async = new MyDbAsyncConnection(faktory);
            var time = new CurrentUtc();
            var oldDb = new AccessDataBase();
            var db = new AccessDataBaseAoT(oldDb, sync, async, time);
            _userValidation = new UserValidation(db, time);
        }

        [Fact]
        public void RegisterUserNullTestIsNull()
        {
            RegisterUser? user = null;

            var result = _userValidation.RegisterUserNull(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void RegisterUserNullTestIsNotNull()
        {
            RegisterUser? user = new();

            var result = _userValidation.RegisterUserNull(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234567")]
        [InlineData("123")]
        public void PasswordLength8TestInvalid(string password)
        {
            var result = _userValidation.PasswordLength8(password);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void PasswordLength8TestValid()
        {
            var password = _validPassword;
            var result = _userValidation.PasswordLength8(password);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void PasswordNoUpperTestInvalid()
        {
            var password = _validPassword.ToLower();
            var result = _userValidation.PasswordNoUpper(password);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void PasswordNoUpperTestValid()
        {
            var password = _validPassword;
            var result = _userValidation.PasswordNoUpper(password);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void PasswordNoLowerTestInvalid()
        {
            var password = _validPassword.ToUpper();
            var result = _userValidation.PasswordNoLower(password);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void PasswordNoLowerTestValid()
        {
            var password = _validPassword;
            var result = _userValidation.PasswordNoLower(password);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void PasswordNoDigitTestInvalid()
        {
            var password = "Password!";
            var result = _userValidation.PasswordNoDigit(password);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void PasswordNoDigitTestValid()
        {
            var password = _validPassword;
            var result = _userValidation.PasswordNoDigit(password);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void PasswordNoSpecialTestInvalid()
        {
            var password = "Password1";
            var result = _userValidation.PasswordNoSpecial(password);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void PasswordNoSpecialTestValid()
        {
            var password = _validPassword;
            var result = _userValidation.PasswordNoSpecial(password);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Theory]
        [InlineData("Password1!", "email.com")]
        [InlineData("Password1", "mail.com.pl")]
        public void PasswordContainEmailTestValid(string password, string confirmPassword)
        {
            var result = _userValidation.PasswordContainEmail(password, confirmPassword);
            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Theory]
        [InlineData("Password1!", "Password1!")]
        [InlineData("Password1!", "Password")]
        [InlineData("Password1!", "Passwo")]
        public void PasswordContainEmailTestInvalid(string password, string confirmPassword)
        {
            var result = _userValidation.PasswordContainEmail(password, confirmPassword);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
    }
}
