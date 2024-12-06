namespace DataBase.Model.EntitiesServer
{
    public static class EnumsList
    {
        public enum Validation
        {
            PasswordNull = 0,
            PasswordLenght = 1,
            PasswordNoUpper = 2,
            PasswordNoLower = 3,
            PasswordNoDigit = 4,
            PasswordNoSpecial = 5,
            PassworContainEmail = 6,
            RegisterUserNull = 7,
            EmailValidFormat = 8,
            EmailExist = 9,
        }
    }
}