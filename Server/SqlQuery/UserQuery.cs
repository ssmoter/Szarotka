using DataBase.Model.EntitiesServer;


namespace Server.SqlQuery
{
    public static class UserQuery
    {
        public static string RegisterNewUser(RegisterUser user)
        {
            string sql = $@"
INSERT INTO {nameof(User)}
(
{nameof(User.Id)},
{nameof(User.CreatedTicks)},
{nameof(User.UpdatedTicks)},
{nameof(User.Name)},
{nameof(User.Description)},
{nameof(User.Email)},
{nameof(User.PhoneNumber)},
{nameof(User.UserType)},
{nameof(User.IsDelete)},
{nameof(User.IsEmailConfirm)},
{nameof(RegisterUser.Password)},
{nameof(User.UserUpdatedId)},
{nameof(User.UserCreatedId)}
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
            return sql;
        }
        public static string EmailConfirmInsert(ConfirmCode user)
        {
            string sql = $@"
INSERT INTO {nameof(ConfirmCode)}
(
{nameof(ConfirmCode.CreatedTicks)},
{nameof(ConfirmCode.UpdatedTicks)},
{nameof(ConfirmCode.UserId)},
{nameof(ConfirmCode.Code)},
{nameof(ConfirmCode.ExpireDate)}
)
VALUES(
{user.CreatedTicks},
{user.UpdatedTicks},
'{user.UserId}',
{user.Code},
{user.ExpireDate}
)
";
            return sql;
        }
        public static string EmailConfirmCheck(int code)
        {
            string sql = $@"
SELECT
{nameof(ConfirmCode.UserId)},
{nameof(ConfirmCode.ExpireDate)}
FROM {nameof(ConfirmCode)}
WHERE
{nameof(ConfirmCode.Code)} = {code}
LIMIT 1
";
            return sql;
        }
        public static string EmailIsConfirmUpdate(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.IsEmailConfirm)} = {user.IsEmailConfirm},
{nameof(User.UpdatedTicks)} = '{user.UpdatedTicks}',
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
";
            return sql;
        }
        public static string RemoveExpireCode(long ticksNow)
        {
            string sql = $@"
DELETE
FROM {nameof(ConfirmCode)}
WHERE
{nameof(ConfirmCode.ExpireDate)} < {ticksNow}
";
            return sql;
        }
        public static string GetEmailFromId(Guid id)
        {
            string sql = $@"
SELECT
{nameof(User.Email)}
FROM {nameof(User)}
WHERE
{nameof(User.Id)} = '{id}'
";
            return sql;
        }






        public static string UpdateName(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Name)} = '{user.Name}',
{nameof(User.UpdatedTicks)} = {user.UpdatedTicks},
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
";
            return sql;
        }
        public static string UpdateDescription(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Description)} = '{user.Description}',
{nameof(User.UpdatedTicks)} = {user.UpdatedTicks},
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
";
            return sql;
        }
        public static string UpdateEmail(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Email)} = '{user.Email}',
{nameof(User.UpdatedTicks)} = {user.UpdatedTicks},
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
"; 
            return sql;
        }
        public static string UpdatePhoneNumber(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.PhoneNumber)} = '{user.PhoneNumber}',
{nameof(User.UpdatedTicks)} = {user.UpdatedTicks},
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
"; 
            return sql;
        }
        public static string UpdateUserType(User user)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.UserType)} = {user.UserType},
{nameof(User.UpdatedTicks)} = {user.UpdatedTicks},
{nameof(User.UserUpdatedId)} = '{user.UserUpdatedId}'
WHERE
{nameof(User.Id)} = '{user.Id}'
";
            return sql;
        }
    }
}
