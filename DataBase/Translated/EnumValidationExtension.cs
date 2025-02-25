using DataBase.Model.EntitiesServer;

namespace DataBase.Translated
{
    public static class EnumValidationExtension
    {
        public static string ValidationToPolish(this EnumsList.Validation validation)
        {
            string message = "";
            switch (validation)
            {
                case EnumsList.Validation.PasswordIsNull:
                    message = "Hasło nie może być puste";
                    break;
                case EnumsList.Validation.PasswordLength8:
                    message = "Hasło musi mieć co najmniej 8 znaków";
                    break;
                case EnumsList.Validation.PasswordNoUpper:
                    message = "Hasło musi zawierać co najmniej jedną wielką literę";
                    break;
                case EnumsList.Validation.PasswordNoLower:
                    message = "Hasło musi zawierać co najmniej małą literę";
                    break;
                case EnumsList.Validation.PasswordNoDigit:
                    message = "Hasło musi zawierać co najmniej jedną cyfrę";
                    break;
                case EnumsList.Validation.PasswordNoSpecial:
                    message = "Hasło musi zawierać co najmniej jeden znak specjalny";
                    break;
                case EnumsList.Validation.PasswordContainEmail:
                    message = "Hasło nie może zawierać adresu email";
                    break;
                case EnumsList.Validation.RegisterUserNull:
                    message = "Nie podano danych do rejestracji";
                    break;
                case EnumsList.Validation.EmailValidFormat:
                    message = "Niepoprawny format adresu email";
                    break;
                case EnumsList.Validation.EmailExist:
                    message = "Adres email jest już zajęty";
                    break;
                case EnumsList.Validation.CodeIsExpire:
                    message = "Kod weryfikacyjny wygasł";
                    break;
                case EnumsList.Validation.CodeNotExist:
                    message = "Kod weryfikacyjny nie istnie";
                    break;
                case EnumsList.Validation.LoginIsNull:
                    message = "Login nie może być pusty";
                    break;
                case EnumsList.Validation.EmailIsNull:
                    message = "Email nie może być pusty";
                    break;
                case EnumsList.Validation.AccountNotFound:
                    message = "Konto nie istnieje";
                    break;
                case EnumsList.Validation.AccountWasDelete:
                    message = "Konto zostało usunięte";
                    break;
                case EnumsList.Validation.AccountEmailIsNotConfirm:
                    message = "Email nie został potwierdzony";
                    break;
                case EnumsList.Validation.NameRequired:
                    message = "Imię jest wymagane";
                    break;
                case EnumsList.Validation.PhoneNumberRequired:
                    message = "Numer telefonu jest wymagany";
                    break;
                case EnumsList.Validation.PhoneNumberInvalid:
                    message = "Niepoprawny numer telefonu";
                    break;
                case EnumsList.Validation.Token:
                    break;
                default:
                    break;
            }
            return message;
        }
        public static string PasswordsValidation(this EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.PasswordIsNull && valid <= EnumsList.Validation.PasswordContainEmail)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }
        public static string EmailValidation(this EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.EmailValidFormat && valid <= EnumsList.Validation.EmailIsNull)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }
        public static string PhoneNumberValidation(this EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.PhoneNumberRequired && valid <= EnumsList.Validation.PhoneNumberInvalid)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }
        public static string NameValidation(this EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.NameRequired && valid <= EnumsList.Validation.NameRequired)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }
        public static string UnclassifiedValidation(this EnumsList.Validation valid, string message)
        {
            switch (valid)
            {
                case EnumsList.Validation.RegisterUserNull:
                case >= EnumsList.Validation.CodeIsExpire and <= EnumsList.Validation.CodeNotExist:
                case EnumsList.Validation.LoginIsNull:
                case >= EnumsList.Validation.AccountNotFound and <= EnumsList.Validation.AccountEmailIsNotConfirm:
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        message += Environment.NewLine;
                    }
                    message += valid.ValidationToPolish();
                    break;
            }
            return message;
        }
    }
}
