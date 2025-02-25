namespace DataBase.Model.EntitiesServer
{
    public static class EnumsList
    {
        public enum Validation
        {
            Token = -1,

            RegisterUserNull = 0,

            PasswordIsNull = 1,
            PasswordLength8 = 2,
            PasswordNoUpper = 3,
            PasswordNoLower = 4,
            PasswordNoDigit = 5,
            PasswordNoSpecial = 6,
            PasswordContainEmail = 10,

            EmailValidFormat = 11,
            EmailExist = 12,
            EmailIsNull = 20,

            CodeIsExpire = 21,
            CodeNotExist = 30,

            LoginIsNull = 31,

            AccountNotFound = 41,
            AccountWasDelete = 42,
            AccountEmailIsNotConfirm = 50,

            NameRequired = 51,
            PhoneNumberRequired = 52,
            PhoneNumberInvalid = 53,


        }
    }
}