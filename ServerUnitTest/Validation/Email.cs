using DataBase.Helper;
using DataBase.Model.EntitiesServer;

using FluentAssertions;

using Server.Validation;

namespace ServerUnitTest.Validation
{
    public class Email
    {
        public static string Path => Constants.GetPathFolder + "\\DataBaseSzarotkaSQLiteUnitTest.db3";

        [Fact]
        public void EmailIsCorrentNull()
        {
            RegisterUser user = null;

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsCorrent(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void EmailIsCorrentEmptyEmail()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "",
            };

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsCorrent(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void EmailIsCorrentDontHaveAt()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "emailgmail.com",
            };

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsCorrent(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void EmailIsCorrentDontHaveDot()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail",
            };

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsCorrent(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void EmailIsCorrentSuccess()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
            };

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsCorrent(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Success);
        }
        [Fact]
        public void EmailIsExistNull()
        {
            RegisterUser user = null;

            IUserValidation _valid = new UserValidation(new());

            var obj = _valid.EmailIsExist(user);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void EmailIsExistTrue()
        {
            RegisterUser user = new RegisterUser()
            {
                Id = Guid.NewGuid(),
                Email = "email@gmail.com",
            };

            var db = new DataBase.Data.AccessDataBase(Path);

            db.DataBase.CreateTable<RegisterUser>();
            db.DataBase.Insert(user);

            IUserValidation _valid = new UserValidation(db);


            var obj = _valid.EmailIsExist(user);

            db.DataBase.DropTable<RegisterUser>();

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);

        }
        [Fact]
        public void EmailIsExistFalse()
        {
            RegisterUser user = new RegisterUser()
            {
                Id = Guid.NewGuid(),
                Email = "email@gmail.com",
            };

            var db = new DataBase.Data.AccessDataBase(Path);

            db.DataBase.CreateTable<RegisterUser>();

            IUserValidation _valid = new UserValidation(db);

            var obj = _valid.EmailIsExist(user);

            db.DataBase.DropTable<RegisterUser>();

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Success);
        }
    }
}
