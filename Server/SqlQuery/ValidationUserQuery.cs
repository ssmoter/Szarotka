using DataBase.Model.EntitiesServer;

namespace Server.SqlQuery
{
    public static class ValidationUserQuery
    {
        public static string SelectEmails()
        {
            string sql = $@"
SELECT {nameof(User)}.{nameof(User.Email)} FROM {nameof(User)}
WHERE {nameof(User)}.{nameof(User.Email)} == ?
";
            return sql;
        }



    }
}
