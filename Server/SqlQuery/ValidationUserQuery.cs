using DataBase.Model.EntitiesServer;

namespace Server.SqlQuery
{
    public static class ValidationUserQuery
    {
        public static string SelectEmails(string email)
        {
            string sql = $@"
SELECT {nameof(User.Email)}
FROM {nameof(User)}
WHERE 
{nameof(User.Email)} == '@{nameof(email)}'
";
            return sql;
        }



    }
}
