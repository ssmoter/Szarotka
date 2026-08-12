using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.SqlQuery;

using System.Globalization;

using System.Text.RegularExpressions;


namespace Server.Validation;

public interface IUserValidation
{
    IValidationException Validation { get; }

    ServerEnums.Result AccountEmailIsNotConfirm(User user);
    ServerEnums.Result AccountNotFound(User? user);
    ServerEnums.Result AccountWasDelete(User user);
    ServerEnums.Result CodeIsExpire(ConfirmCode code);
    ServerEnums.Result CodeNotExist(ConfirmCode? code);
    /// <summary>
    /// sprawdza czy email istnieje w bazie danych
    /// DZIAŁA TYLKO NA SERWERZE
    /// </summary>
    /// <param name="email">email do sprawdzenia</param>
    /// <returns></returns>
    Task<ServerEnums.Result> EmailExist(string email);
    ServerEnums.Result EmailIsNull(string email);
    ServerEnums.Result EmailValidFormat(string email);
    ServerEnums.Result LoginIsNull(LoginUser? login);
    ServerEnums.Result NameRequired(string name);
    ServerEnums.Result PasswordContainEmail(string password, string email);
    ServerEnums.Result PasswordLength8(string password);
    ServerEnums.Result PasswordNoDigit(string password);
    ServerEnums.Result PasswordNoLower(string password);
    ServerEnums.Result PasswordNoSpecial(string password);
    ServerEnums.Result PasswordNoUpper(string password);
    ServerEnums.Result PasswordIsNull(string password);
    ServerEnums.Result PhoneNumberPattern(string phoneNumber);
    ServerEnums.Result PhoneNumberRequired(string phoneNumber);
    ServerEnums.Result RegisterUserNull(RegisterUser? user);
    void AddToken(string token);
}

public class UserValidation : IUserValidation
{
    private readonly IAccessDataBaseAoT _db;
    private readonly ITimeService _timeService;
    private readonly string _special = "!@#$%^&*()_+-=[]{}|;:'\",.<>?/\\`~";
    private readonly string _phoneNumberPattern = @"^\+?[1-9]\d{1,14}$"; // E.164 format

    public IValidationException Validation { get; private set; } = new ValidationException();
    public UserValidation(IAccessDataBaseAoT db, ITimeService timeService, IValidationException? validation = null)
    {
        _db = db;
        _timeService = timeService;
        if (validation is not null)
        {
            Validation = validation;
        }
    }
    public void AddToken(string token)
    {
        Validation.AddError(token, EnumsList.Validation.Token);
    }

    public ServerEnums.Result RegisterUserNull(RegisterUser? user)
    {
        var result = ServerEnums.Result.Success;
        if (user is null)
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser)} is required";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.RegisterUserNull);
        }
        return result;
    }
    public ServerEnums.Result PasswordIsNull(string password)
    {
        var result = ServerEnums.Result.Success;
        if (string.IsNullOrWhiteSpace(password))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} is null";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordIsNull);
        }
        return result;
    }
    public ServerEnums.Result PasswordLength8(string password)
    {
        var result = ServerEnums.Result.Success;
        if (password.Length < 8)
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} is shorter than 8 characters";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordLength8);
        }
        return result;
    }
    public ServerEnums.Result PasswordNoUpper(string password)
    {
        var result = ServerEnums.Result.Success;
        if (!password.Any(char.IsUpper))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} don't have 1 upper characters";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordNoUpper);
        }
        return result;
    }
    public ServerEnums.Result PasswordNoLower(string password)
    {
        var result = ServerEnums.Result.Success;
        if (!password.Any(char.IsLower))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} don't have 1 lower characters";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordNoLower);
        }
        return result;
    }
    public ServerEnums.Result PasswordNoDigit(string password)
    {
        var result = ServerEnums.Result.Success;
        if (!password.Any(char.IsDigit))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} don't have 1 digit characters";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordNoDigit);
        }
        return result;
    }
    public ServerEnums.Result PasswordNoSpecial(string password)
    {
        var result = ServerEnums.Result.Success;
        if (!password.Any(x => _special.Contains(x)))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} don't have 1 special characters";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordNoSpecial);
        }
        return result;
    }
    public ServerEnums.Result PasswordContainEmail(string password, string email)
    {
        var result = ServerEnums.Result.Success;
        if (password.Contains(email))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Password)} contains {nameof(RegisterUser.Email)}";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PasswordContainEmail);
        }
        return result;
    }



    /// <summary>
    /// kod od microsoft
    /// </summary>
    /// <param name="emailTest"></param>
    /// <returns></returns>
    public ServerEnums.Result EmailValidFormat(string email)
    {
        var result = ServerEnums.Result.Success;
        var emailCopy = email;

        try
        {
            // Normalize the domain
            emailCopy = Regex.Replace(emailCopy, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            static string DomainMapper(Match match)
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
            result = ServerEnums.Result.Error;
        }
        catch (ArgumentException)
        {
            result = ServerEnums.Result.Error;
        }

        try
        {
            var regexResult = Regex.IsMatch(emailCopy,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            result = regexResult ? ServerEnums.Result.Success : ServerEnums.Result.Error;
        }
        catch (RegexMatchTimeoutException)
        {
            result = ServerEnums.Result.Error;
        }

        if (result == ServerEnums.Result.Error)
        {
            string message = $"{nameof(RegisterUser.Email)} is not a valid format";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.EmailValidFormat);
        }

        return result;
    }
    public ServerEnums.Result EmailIsNull(string email)
    {
        var result = ServerEnums.Result.Success;
        if (string.IsNullOrWhiteSpace(email))
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Email)} is null";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.EmailIsNull);
        }
        return result;
    }
    /// <summary>
    /// sprawdza czy email istnieje w bazie danych
    /// DZIAŁA TYLKO NA SERWERZE
    /// </summary>
    /// <param name="email">email do sprawdzenia</param>
    /// <returns></returns>
    public async Task<ServerEnums.Result> EmailExist(string email)
    {
        var sql = ValidationUserQuery.SelectEmails(email);
        var emails = await _db.DbAsyncAoT.QueryAsync<User>(sql, new()
        {
            [nameof(email)] = email
        });

        var result = ServerEnums.Result.Success;

        if (emails.Any())
        {
            result = ServerEnums.Result.Error;
            string message = $"{nameof(RegisterUser.Email)} arleady exist";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.EmailExist);
        }
        return result;
    }



    public ServerEnums.Result CodeIsExpire(ConfirmCode code)
    {
        var result = ServerEnums.Result.Success;

        var ticks = _timeService.UtcNow().Ticks;
        if (code.ExpireDate < ticks)
        {
            var time = new TimeSpan(code.ExpireDate - ticks);
            result = ServerEnums.Result.Error;
            string message = $"The {nameof(ConfirmCode.ExpireDate)} time has passed, {time.TotalSeconds}s to late";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.CodeIsExpire);
        }
        return result;
    }
    public ServerEnums.Result CodeNotExist(ConfirmCode? code)
    {
        var result = ServerEnums.Result.Success;

        if (code is null)
        {
            result = ServerEnums.Result.Error;
            string message = $"Not find a selected {nameof(ConfirmCode.Code)}";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.CodeNotExist);
        }
        return result;
    }


    public ServerEnums.Result LoginIsNull(LoginUser? login)
    {
        var result = ServerEnums.Result.Success;
        if (login is null)
        {
            result = ServerEnums.Result.Error;
            var message = $"{nameof(LoginUser)} is null";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.LoginIsNull);
        }
        return result;
    }
    public ServerEnums.Result AccountNotFound(User? user)
    {
        var result = ServerEnums.Result.Success;
        if (user is null)
        {
            result = ServerEnums.Result.Error;
            var message = $"Account not found";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.AccountNotFound);
        }
        return result;
    }
    public ServerEnums.Result AccountWasDelete(User user)
    {
        var result = ServerEnums.Result.Success;
        if (user.IsDelete)
        {
            result = ServerEnums.Result.Error;
            var message = $"Account was delete";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.AccountWasDelete);
        }
        return result;
    }
    public ServerEnums.Result AccountEmailIsNotConfirm(User user)
    {
        var result = ServerEnums.Result.Success;
        if (!user.IsEmailConfirm)
        {
            result = ServerEnums.Result.Error;
            var message = $"Confirm your email, new code was sent";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.AccountEmailIsNotConfirm);
        }
        return result;
    }

    public ServerEnums.Result NameRequired(string name)
    {
        var result = ServerEnums.Result.Success;
        if (string.IsNullOrWhiteSpace(name))
        {
            result = ServerEnums.Result.Error;
            var message = $"{nameof(name)} is required";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.NameRequired);
        }
        return result;
    }
    public ServerEnums.Result PhoneNumberRequired(string phoneNumber)
    {
        var result = ServerEnums.Result.Success;
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            result = ServerEnums.Result.Error;
            var message = $"{nameof(phoneNumber)} is required";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PhoneNumberRequired);
        }
        return result;
    }
    public ServerEnums.Result PhoneNumberPattern(string phoneNumber)
    {
        var result = ServerEnums.Result.Success;
        if (!Regex.IsMatch(phoneNumber, _phoneNumberPattern))
        {
            result = ServerEnums.Result.Error;
            var message = $"{nameof(phoneNumber)} is not valid";
            Console.WriteLine(message);
            Validation.AddError(message, EnumsList.Validation.PhoneNumberInvalid);
        }
        return result;
    }

}

