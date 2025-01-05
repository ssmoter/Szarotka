using DataBase.Model.EntitiesServer;

namespace Server.SqlQuery
{
    public class LoginQuery
    {
        public static string In(LoginUser user)
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
{nameof(User.IsEmailConfirm)}
FROM {nameof(User)}
WHERE 
{nameof(User.Email)} = '{user.Email}'
AND
{nameof(RegisterUser.Password)} = '{user.Password}'
LIMIT 1
";
            return sql;
        }
        public static string UpdateRememberMe(User user)
        {
            string sql = @$"
        UPDATE {nameof(User)}
        SET 
        {nameof(User.RememberMe)} = {user.RememberMe}
        WHERE 
        {nameof(User.Id)} = '{user.Id}'
        ";
            return sql;
        }

        public static string InFromId(string id)
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
{nameof(User.RememberMe)}
FROM {nameof(User)}
WHERE 
{nameof(User.Id)} = '{id}'
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
