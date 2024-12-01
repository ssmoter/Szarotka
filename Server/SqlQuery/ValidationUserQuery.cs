using DataBase.Model.EntitiesServer;

namespace Server.SqlQuery
{
    public static class ValidationUserQuery
    {
        public static string SelectEmails()
        {
            string sql = $@"
SELECT {nameof(RegisterUser)}.{nameof(RegisterUser.Email)} FROM {nameof(RegisterUser)}
WHERE {nameof(RegisterUser)}.{nameof(RegisterUser.Email)} == '?'
";
            return sql;
        }



    }
}
