namespace DataBase.Model.EntitiesServer
{
    public static class EnumsList
    {
        public enum Validation
        {
            RegisterUserNull = 0,

            PasswordIsNull = 1,
            PasswordLength8 = 2,
            PasswordNoUpper = 3,
            PasswordNoLower = 4,
            PasswordNoDigit = 5,
            PasswordNoSpecial = 6,
            PasswordsContainEmail = 10,

            EmailValidFormat = 11,
            EmailExist = 12,
            EmailIsNull = 20,

            EmailCodeIsExpire = 21,
            EmailCodeNotExist = 30,

            LoginIsNull = 31,

            AccountNotFound = 41,
            AccountWasDelete = 42,
            AccountEmailIsNotConfirm = 50,
        }
    }
}