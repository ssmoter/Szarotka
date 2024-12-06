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
        ServerEnums.ValidationResult EmailIsCorrent(RegisterUser? user);
        ServerEnums.ValidationResult EmailIsExist(RegisterUser? user);
        ServerEnums.ValidationResult PasswordValid(RegisterUser? user, ref ValidationException error);
    }

    public class UserValidation : IUserValidation
    {
        private readonly AccessDataBase _db;
        private readonly string _special = "!@#$%^&*()_+-=[]{}|;:'\",.<>?/\\`~";
        public UserValidation(AccessDataBase db)
        {
            _db = db;
        }

        //kod od microsoft
        public ServerEnums.ValidationResult EmailIsCorrent(RegisterUser? user)
        {

            if (user is null)
                return ServerEnums.ValidationResult.Error;

            var email = user.Email;

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
        public ServerEnums.ValidationResult EmailIsExist(RegisterUser? user)
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
        public ServerEnums.ValidationResult PasswordValid(RegisterUser? user, ref ValidationException error)
        {
            ServerEnums.ValidationResult result = ServerEnums.ValidationResult.Success;

            if (user is null)
            {
                string message = $"{nameof(RegisterUser)} is null";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordNull);
                return ServerEnums.ValidationResult.Error;
            }

            var password = user.Password;

            if (password.Length < 8)
            {
                string message = $"{nameof(RegisterUser.Password)} is shorter than 8 characters";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordLenght);
                result = ServerEnums.ValidationResult.Error;
            }

            bool upper = false;
            bool lower = false;
            bool digit = false;
            bool special = false;
            foreach (var item in password)
            {
                if (char.IsUpper(item))
                    upper = true;
                if (char.IsLower(item))
                    lower = true;
                if (char.IsDigit(item))
                    digit = true;
                if (_special.Contains(item))
                    special = true;
            }
            if (!upper)
            {
                string message = $"{nameof(RegisterUser.Password)} don't have 1 upper characters";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordNoUpper);
                result = ServerEnums.ValidationResult.Error;
            }
            if (!lower)
            {
                string message = $"{nameof(RegisterUser.Password)} don't have 1 lower characters";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordNoLower);
                result = ServerEnums.ValidationResult.Error;
            }
            if (!digit)
            {
                string message = $"{nameof(RegisterUser.Password)} don't have 1 digit characters";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordNoDigit);
                result = ServerEnums.ValidationResult.Error;
            }
            if (!special)
            {
                string message = $"{nameof(RegisterUser.Password)} don't have 1 special characters";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PasswordNoSpecial);
                result = ServerEnums.ValidationResult.Error;
            }
            if (password.Contains(user.Email))
            {
                string message = $"{nameof(RegisterUser.Password)} contains {nameof(RegisterUser.Email)}";
                Console.WriteLine(message);
                error.AddError(message, EnumsList.Validation.PassworContainEmail);
                result = ServerEnums.ValidationResult.Error;
            }
            return result;
        }


    }
}
