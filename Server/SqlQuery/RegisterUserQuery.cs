using DataBase.Model.EntitiesServer;


namespace Server.SqlQuery
{
    public static class RegisterUserQuery
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
{nameof(RegisterUser.Password)}
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
'{user.Password}'
)
";
            return sql;
        }
        public static string EmailConfirmInsert(RegisterConfirmEmailUser user)
        {
            string sql = $@"
INSERT INTO {nameof(RegisterConfirmEmailUser)}
(
{nameof(RegisterConfirmEmailUser.CreatedTicks)},
{nameof(RegisterConfirmEmailUser.UpdatedTicks)},
{nameof(RegisterConfirmEmailUser.UserId)},
{nameof(RegisterConfirmEmailUser.Code)},
{nameof(RegisterConfirmEmailUser.ExpireDate)}
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
{nameof(RegisterConfirmEmailUser.UserId)},
{nameof(RegisterConfirmEmailUser.ExpireDate)}
FROM {nameof(RegisterConfirmEmailUser)}
WHERE 
{nameof(RegisterConfirmEmailUser.Code)} = {code}
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
  {nameof(User.UpdatedTicks)} = '{user.UpdatedTicks}'
WHERE
{nameof(User.Id)} = '{user.Id}'
";
            return sql;
        }
        public static string RemoveExpireCode()
        {
            string sql = $@"
DELETE 
FROM {nameof(RegisterConfirmEmailUser)}
WHERE 
{nameof(RegisterConfirmEmailUser.ExpireDate)} < {DateTime.UtcNow.Ticks}
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

    }
}
