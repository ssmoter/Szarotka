using DataBase.Model.EntitiesServer;

namespace Server.SqlQuery
{
    public class LoginQuery
    {
        public static string In(string Email, string Password)
        {
            string sql = @$"
SELECT
{nameof(User.Id)},
{nameof(User.Name)},
{nameof(User.Email)},
{nameof(User.CreatedTicks)},
{nameof(User.UpdatedTicks)},
{nameof(User.UserType)},
{nameof(User.Description)},
{nameof(User.PhoneNumber)},
{nameof(User.IsDelete)},
{nameof(User.IsEmailConfirm)},
{nameof(User.RememberMe)},
{nameof(User.UserUpdatedId)},
{nameof(User.UserCreatedId)}
FROM {nameof(User)}
WHERE
{nameof(User.Email)} = @{nameof(Email)}
AND
{nameof(RegisterUser.Password)} = @{nameof(Password)}
LIMIT 1
";
            return sql;
        }
        public static string UpdateRememberMe(bool RememberMe, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = @$"
UPDATE {nameof(User)}
SET
{nameof(User.RememberMe)} = @{nameof(RememberMe)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string InFromId(string Id)
        {
            string sql = @$"
SELECT
{nameof(User.Id)},
{nameof(User.Name)},
{nameof(User.Email)},
{nameof(User.CreatedTicks)},
{nameof(User.UpdatedTicks)},
{nameof(User.UserType)},
{nameof(User.Description)},
{nameof(User.PhoneNumber)},
{nameof(User.IsDelete)},
{nameof(User.IsEmailConfirm)},
{nameof(User.RememberMe)},
{nameof(User.UserUpdatedId)},
{nameof(User.UserCreatedId)}
FROM {nameof(User)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
LIMIT 1
";
            return sql;
        }
        public static string PublicUser(string Id)
        {
            string sql = @$"
SELECT
{nameof(User.Id)},
{nameof(User.Name)},
{nameof(User.CreatedTicks)},
{nameof(User.UpdatedTicks)},
{nameof(User.UserType)},
{nameof(User.Description)},
{nameof(User.PhoneNumber)},
{nameof(User.IsDelete)},
{nameof(User.IsEmailConfirm)},
{nameof(User.UserUpdatedId)},
{nameof(User.UserCreatedId)}
FROM {nameof(User)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
LIMIT 1
";
            return sql;
        }

        public static string Out(string user)
        {
            return user;
        }
    }
}
