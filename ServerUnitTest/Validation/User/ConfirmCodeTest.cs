using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.Validation;

namespace ServerUnitTest.Validation.User
{
    public class ConfirmCodeTest
    {
        private readonly IUserValidation _userValidation;
        private readonly ITimeService _timeService;

        public ConfirmCodeTest()
        {
            var utc = new CurrentUtc();
            _timeService = utc;
            _userValidation = new UserValidation(new AccessDataBase(), _timeService);
        }

        [Fact]
        public void CodeNotExistTestIsNull()
        {
            ConfirmCode? user = null;

            var result = _userValidation.CodeNotExist(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void CodeNotExistTestIsNotNull()
        {
            ConfirmCode? user = new();

            var result = _userValidation.CodeNotExist(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }

        [Fact]
        public void CodeIsExpireTestInvalid()
        {
            ConfirmCode user = new()
            {
                ExpireDate = _timeService.UtcNow().AddMinutes(-10).Ticks,
            };
            var result = _userValidation.CodeIsExpire(user);

            Assert.Equal(ServerEnums.Result.Error, result);
        }
        [Fact]
        public void CodeIsExpireTestValid()
        {
            ConfirmCode user = new()
            {
                ExpireDate = _timeService.UtcNow().AddMinutes(10).Ticks,
            };

            var result = _userValidation.CodeIsExpire(user);

            Assert.Equal(ServerEnums.Result.Success, result);
        }

    }
}
