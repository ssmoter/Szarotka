namespace DataBase.Model.EntitiesServer
{
    public static class EnumsList
    {
        public enum Validation
        {
            PasswordIsNull = 0,
            PasswordLenght = 1,
            PasswordNoUpper = 2,
            PasswordNoLower = 3,
            PasswordNoDigit = 4,
            PasswordNoSpecial = 5,
            PassworContainEmail = 6,

            RegisterUserNull = 7,

            EmailValidFormat = 8,
            EmailExist = 9,

            EmailCodeIsExpire = 10,
            EmailCodeNotExist = 11,

            LoginIsNull = 12,
            EmailIsNull = 13,

            AccountNotFound = 14,
            AccountWasDelete = 15,
            AccountEmailIsNotConfirm = 16,
        }
    }
}