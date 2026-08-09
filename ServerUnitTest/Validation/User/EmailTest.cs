using DataBase.Data;
using DataBase.Service;

using Server.Model;
using Server.Validation;
using DataBase.Data.MySqliteConnection;

namespace ServerUnitTest.Validation.User
{
    public class EmailTest
    {
        private readonly IUserValidation _userValidation;
        private const string _validEmail = "user@example.com";

        public EmailTest()
        {
            var faktory = new SqliteConnectionFactory();
            var sync = new MyDbConnection(faktory);
            var async = new MyDbAsyncConnection(faktory);
            var time = new CurrentUtc();
            var oldDb = new AccessDataBase();
            var db = new AccessDataBaseAoT(oldDb, sync, async, time);
            _userValidation = new UserValidation(db, time);
        }

        [Theory]
        [InlineData("user@example.")]
        [InlineData("user.")]
        [InlineData("user.com")]
        [InlineData("user@.com")]
        public void EmailValidFormatTestInvalid(string email)
        {
            var result = _userValidation.EmailValidFormat(email);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Theory]
        [InlineData(_validEmail)]
        [InlineData("test@gmail.com")]
        [InlineData("test123@onet.pl")]
        [InlineData("test.test@gmail.com")]
        public void EmailValidFormatTestValid(string email)
        {
            var result = _userValidation.EmailValidFormat(email);
            Assert.Equal(ServerEnums.Result.Success, result);
        }

        [Theory]
        [InlineData(_validEmail)]
        [InlineData("test@gmail.com")]
        [InlineData("test123@onet.pl")]
        [InlineData("test.test@gmail.com")]
        public void EmailIsNullTestValid(string email)
        {
            var result = _userValidation.EmailIsNull(email);
            Assert.Equal(ServerEnums.Result.Success, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void EmailIsNullTestInvalid(string email)
        {
            var result = _userValidation.EmailIsNull(email);
            Assert.Equal(ServerEnums.Result.Error, result);
        }
    }
}
