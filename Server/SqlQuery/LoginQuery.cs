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
{nameof(User.PhoneNumber)}
FROM {nameof(User)}
WHERE 
{nameof(User.Email)} = '{user.Email}'
AND
{nameof(RegisterUser.Password)} = '{user.Password}'
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
