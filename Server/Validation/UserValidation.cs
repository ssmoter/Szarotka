using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.SqlQuery;

using System.Globalization;

using System.Text.RegularExpressions;


namespace Server.Validation
{
    public interface IUserValidation
    {
        ServerEnums.ValidationResult EmailIsCorrent(RegisterUser user);
        ServerEnums.ValidationResult EmailIsExist(RegisterUser user);
    }

    public class UserValidation : IUserValidation
    {
        private readonly AccessDataBase _db;
        public UserValidation(AccessDataBase db)
        {
            _db = db;
        }

        //kod od microsoft
        public ServerEnums.ValidationResult EmailIsCorrent(RegisterUser user)
        {
            var email = user.Email;

            if (user is null)
                return ServerEnums.ValidationResult.Error;


            if (string.IsNullOrWhiteSpace(email))
                return ServerEnums.ValidationResult.Error;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return ServerEnums.ValidationResult.Error;
            }
            catch (ArgumentException)
            {
                return ServerEnums.ValidationResult.Error;
            }

            try
            {
                var regexResult = Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                return regexResult ? ServerEnums.ValidationResult.Success : ServerEnums.ValidationResult.Error;
            }
            catch (RegexMatchTimeoutException)
            {
                return ServerEnums.ValidationResult.Error;
            }
        }

        public ServerEnums.ValidationResult EmailIsExist(RegisterUser user)
        {
            if (user is null)
                return ServerEnums.ValidationResult.Error;

            var emails = _db.DataBase.Query<User>(ValidationUserQuery.SelectEmails(), user.Email);

            if (emails is null)
                return ServerEnums.ValidationResult.Success;

            if (emails.Count > 0)
                return ServerEnums.ValidationResult.Error;


            return ServerEnums.ValidationResult.Success;
        }

        //public ServerEnums.ValidationResult PasswordValid(RegisterUser user)
        //{

        //}

    }
}
