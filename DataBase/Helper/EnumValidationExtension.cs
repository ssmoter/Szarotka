namespace DataBase.Helper
{
    public static class EnumValidationExtension
    {
        public static string ValidationToPolish(this Model.EntitiesServer.EnumsList.Validation validation)
        {
            string message = "";
            switch (validation)
            {
                case Model.EntitiesServer.EnumsList.Validation.PasswordIsNull:
                    message = "Hasło nie może być puste";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordLength8:
                    message = "Hasło musi mieć co najmniej 8 znaków";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordNoUpper:
                    message = "Hasło musi zawierać co najmniej jedną wielką literę";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordNoLower:
                    message = "Hasło musi zawierać co najmniej małą literę";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordNoDigit:
                    message = "Hasło musi zawierać co najmniej jedną cyfrę";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordNoSpecial:
                    message = "Hasło musi zawierać co najmniej jeden znak specjalny";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.PasswordsContainEmail:
                    message = "Hasło nie może zawierać adresu email";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.RegisterUserNull:
                    message = "Nie podano danych do rejestracji";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.EmailValidFormat:
                    message = "Niepoprawny format adresu email";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.EmailExist:
                    message = "Adres email jest już zajęty";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.EmailCodeIsExpire:
                    message = "Kod weryfikacyjny wygasł";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.EmailCodeNotExist:
                    message = "Kod weryfikacyjny nie istnie";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.LoginIsNull:
                    message = "Login nie może być pusty";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.EmailIsNull:
                    message = "Email nie może być pusty";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.AccountNotFound:
                    message = "Konto nie istnieje";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.AccountWasDelete:
                    message = "Konto zostało usunięte";
                    break;
                case Model.EntitiesServer.EnumsList.Validation.AccountEmailIsNotConfirm:
                    message = "Email nie został potwierdzony";
                    break;
                default:
                    break;
            }
            return message;
        }


    }
}
