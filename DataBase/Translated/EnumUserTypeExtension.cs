using DataBase.Model.EntitiesServer;

namespace DataBase.Translated
{
    public static class EnumUserTypeExtension
    {
        public static string ToFriendlyString(this UserType me)
        {
            string result="";
            switch (me)
            {
                case UserType.Driver:
                    result = "Kierowca";
                    break;
                case UserType.Confectioner:
                    result = "Cukiernik";
                    break;
                case UserType.Baker:
                    result = "Piekarz";
                    break;
            }
            return result;
        }
    }
}
