using Server.SqlQuery;

namespace ServerUnitTest.SQL
{
    public class ValidationUserQueryTest
    {
        [Fact]
        public void SelectEmailsTest()
        {
            var email = "email@gmail.com";
            string sql = ValidationUserQuery.SelectEmails(email);
            string expected = $@"
SELECT Email
FROM User
WHERE 
Email == '{email}'
";
            Assert.Equal(expected, sql);
        }
    }
}
