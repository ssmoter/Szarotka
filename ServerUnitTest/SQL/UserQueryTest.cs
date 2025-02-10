using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.SqlQuery;

namespace ServerUnitTest.SQL
{
    public class UserQueryTest
    {
        [Fact]
        public void RegisterNewUserTest()
        {
            RegisterUser user = new RegisterUser
            {
                Id = Guid.Empty,
                CreatedTicks = 1,
                UpdatedTicks = 1,
                Name = "Name",
                Description = "Description",
                Email = "Email",
                PhoneNumber = "PhoneNumber",
                UserType = UserType.Driver,
                IsDelete = false,
                IsEmailConfirm = false,
                Password = "Password",
            };
            var sql = UserQuery.RegisterNewUser(user);
            string expected = $@"
INSERT INTO UserP
(
Id,
CreatedTicks,
UpdatedTicks,
Name,
Description,
Email,
PhoneNumber,
UserType,
IsDelete,
IsEmailConfirm,
Password,
UserUpdatedId,
UserCreatedId
)
VALUES(
'{user.Id}',
{user.CreatedTicks},
{user.UpdatedTicks},
'{user.Name}',
'{user.Description}',
'{user.Email}',
'{user.PhoneNumber}',
'{user.UserType}',
'{user.IsDelete}',
'{user.IsEmailConfirm}',
'{user.Password}',
'{user.Id}',
'{user.Id}'
)
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void EmailConfirmInsertTest()
        {
            ConfirmCode user = new ConfirmCode
            {
                CreatedTicks = 1,
                UpdatedTicks = 1,
                UserId = Guid.Empty,
                Code = 1,
                ExpireDate = 1,
            };
            var sql = UserQuery.EmailConfirmInsert(user);
            string expected = $@"
INSERT INTO ConfirmCode
(
CreatedTicks,
UpdatedTicks,
UserId,
Code,
ExpireDate
)
VALUES(
{user.CreatedTicks},
{user.UpdatedTicks},
'{user.UserId}',
{user.Code},
{user.ExpireDate}
)
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void EmailConfirmCheckTest()
        {
            int code = 1;
            var sql = UserQuery.EmailConfirmCheck(code);
            string expected = $@"
SELECT
UserId,
ExpireDate
FROM ConfirmCode
WHERE
Code = {code}
LIMIT 1
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void EmailIsConfirmUpdateTest()
        {
            User user = new User
            {
                IsEmailConfirm = false,
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.EmailIsConfirmUpdate(user);
            string expected = $@"
UPDATE UserP
SET
IsEmailConfirm = {user.IsEmailConfirm},
UpdatedTicks = '{user.UpdatedTicks}',
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void RemoveExpireCodeTest()
        {
            ITimeService time = new CurrentUtc();
            long ticks = time.UtcNow().Ticks;
            var sql = UserQuery.RemoveExpireCode(ticks);
            string expected = $@"
DELETE
FROM ConfirmCode
WHERE
ExpireDate < {ticks}
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void GetEmailFromIdTest()
        {
            Guid id = Guid.Empty;
            var sql = UserQuery.GetEmailFromId(id);
            string expected = $@"
SELECT
Email
FROM UserP
WHERE
Id = '{id}'
";
            Assert.Equal(expected, sql);
        }



        [Fact]
        public void UpdateNameTest()
        {
            User user = new User
            {
                Name = "Name",
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.UpdateName(user);
            string expected = $@"
UPDATE UserP
SET
Name = '{user.Name}',
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }

        [Fact]
        public void UpdateDescriptionTest()
        {
            User user = new User
            {
                Description = "Description",
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.UpdateDescription(user);
            string expected = $@"
UPDATE UserP
SET
Description = '{user.Description}',
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }
        [Fact]
        public void UpdateEmailTest()
        {
            User user = new User
            {
                Email = "Email",
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.UpdateEmail(user);
            string expected = $@"
UPDATE UserP
SET
Email = '{user.Email}',
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }
        [Fact]
        public void UpdatePhoneNumberTest()
        {
            User user = new User
            {
                PhoneNumber = "PhoneNumber",
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.UpdatePhoneNumber(user);
            string expected = $@"
UPDATE UserP
SET
PhoneNumber = '{user.PhoneNumber}',
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }
        [Fact]
        public void UpdateUserTypeTest()
        {
            User user = new User
            {
                UserType = UserType.Confectioner,
                UpdatedTicks = 1,
                UserUpdatedId = Guid.Empty,
                Id = Guid.Empty,
            };
            var sql = UserQuery.UpdateUserType(user);
            string expected = $@"
UPDATE UserP
SET
UserType = {user.UserType},
UpdatedTicks = {user.UpdatedTicks},
UserUpdatedId = '{user.UserUpdatedId}'
WHERE
Id = '{user.Id}'
";
            Assert.Equal(expected, sql);
        }










    }
}
