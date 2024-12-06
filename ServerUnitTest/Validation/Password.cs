using DataBase.Model.EntitiesServer;

using FluentAssertions;

using Server.Model;
using Server.Validation;

namespace ServerUnitTest.Validation
{
    public class Password
    {
        [Fact]
        public void ReturnValidNull()
        {
            RegisterUser user = null;

            IUserValidation _valid = new UserValidation(new());
            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidLength7()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "123456"
            };

            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidNoUpper()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gma"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidNoLower()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gma".ToUpper()
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidNoSpecial()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "emailgMa1"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidContainEmail()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gmail.comasd1!S"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);

            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Error);
        }
        [Fact]
        public void ReturnValidSuccess()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "1Email@gmail.com"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            obj.Should().Be(Server.Model.ServerEnums.ValidationResult.Success);
        }



        [Fact]
        public void RefValidNull()
        {
            RegisterUser user = null;

            IUserValidation _valid = new UserValidation(new());
            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PasswordNull).Should().Be(1);
        }
        [Fact]
        public void RefValidLength7()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "123456"
            };

            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PasswordLenght).Should().Be(1);
        }
        [Fact]
        public void RefValidNoUpper()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gma"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PasswordNoUpper).Should().Be(1);

        }
        [Fact]
        public void RefValidNoLower()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gma".ToUpper()
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PasswordNoLower).Should().Be(1);

        }
        [Fact]
        public void RefValidNoSpecial()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "emailgMa1"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PasswordNoSpecial).Should().Be(1);

        }
        [Fact]
        public void RefValidContainEmail()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "email@gmail.comasd1!S"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count(x => x.Validation == EnumsList.Validation.PassworContainEmail).Should().Be(1);

        }
        [Fact]
        public void ValidSuccess()
        {
            RegisterUser user = new RegisterUser()
            {
                Email = "email@gmail.com",
                Password = "1Email@gmail.com"
            };
            IUserValidation _valid = new UserValidation(new());

            var valid = new ValidationException();
            var obj = _valid.PasswordValid(user, ref valid);
            valid.ValidationErrors.Count().Should().Be(0);
        }
    }
}
