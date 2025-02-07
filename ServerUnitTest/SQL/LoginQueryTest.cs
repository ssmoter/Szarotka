using DataBase.Model.EntitiesServer;

using Server.SqlQuery;

namespace ServerUnitTest.SQL
{
    public class LoginQueryTest
    {
        [Fact]
        public void InTest()
        {
            LoginUser user = new LoginUser
            {
                Email = "email@gmail.com",
                Password = "password",
                RememberMe = true
            };
            var sql = LoginQuery.In(user);
            var expected = $@"
SELECT
Id,
Name,
Email,
CreatedTicks,
UpdatedTicks,
UserType,
Description,
PhoneNumber,
IsDelete,
IsEmailConfirm
FROM User
WHERE
Email = '{user.Email}'
AND
Password = '{user.Password}'
LIMIT 1
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void UpdateRememberMeTest()
        {
            User user = new User
            {
                RememberMe = true,
                UpdatedTicks = DateTime.Now.Ticks,
            };
            var sql = LoginQuery.UpdateRememberMe(user);
            var expected = $@"
UPDATE User
SET
RememberMe = {user.RememberMe},
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);

        }

        [Fact]
        public void InFromIdTest()
        {
            string id = Guid.Empty.ToString();
            var sql = LoginQuery.InFromId(id);
            var expected = $@"
SELECT
Id,
Name,
Email,
CreatedTicks,
UpdatedTicks,
UserType,
Description,
PhoneNumber,
IsDelete,
IsEmailConfirm,
RememberMe
FROM User
WHERE
Id = '{id}'
LIMIT 1
";
            Assert.Equal(expected, sql);
        }
    }
}
