using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.Validation;
using DataBase.Data.MySqliteConnection;

namespace ServerUnitTest.Validation.User
{
    public class LoginTest
    {
        private readonly IUserValidation _userValidation;

        public LoginTest()
        {
            var faktory = new SqliteConnectionFactory();
            var sync = new MyDbConnection(faktory);
            var async = new MyDbAsyncConnection(faktory);
            var time = new CurrentUtc();
            var oldDb = new AccessDataBase();
            var db = new AccessDataBaseAoT(oldDb, sync, async, time);
            _userValidation = new UserValidation(db,time);
        }

        [Fact]
        public void LoginIsNullTestInvalid()
        {
            LoginUser? user = null;

            var result = _userValidation.LoginIsNull(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void RegisterUserNullTestValid()
        {
            LoginUser? user = new();

            var result = _userValidation.LoginIsNull(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }

        [Fact]
        public void AccountNotFoundTestInvalid()
        {
            DataBase.Model.EntitiesServer.User? user = null;

            var result = _userValidation.AccountNotFound(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void AccountNotFoundTestValid()
        {
            DataBase.Model.EntitiesServer.User? user = new();

            var result = _userValidation.AccountNotFound(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void AccountWasDeleteTestInvalid()
        {
            DataBase.Model.EntitiesServer.User user = new()
            {
                IsDelete = true,
            };

            var result = _userValidation.AccountWasDelete(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void AccountWasDeleteTestValid()
        {
            DataBase.Model.EntitiesServer.User user = new()
            {
                IsDelete = false,
            };
            var result = _userValidation.AccountWasDelete(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }
        [Fact]
        public void AccountEmailIsNotConfirmTestInvalid()
        {
            DataBase.Model.EntitiesServer.User user = new()
            {
                IsEmailConfirm = false,
            };

            var result = _userValidation.AccountEmailIsNotConfirm(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void AccountEmailIsNotConfirmTestValid()
        {
            DataBase.Model.EntitiesServer.User user = new()
            {
                IsEmailConfirm = true,
            };
            var result = _userValidation.AccountEmailIsNotConfirm(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }
    }
}
