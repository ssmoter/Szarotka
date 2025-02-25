using System.Globalization;
using DataBase.Translated;


namespace Shared.Converters
{
    public class UserTypeTranslated : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataBase.Model.EntitiesServer.UserType userType)
            {
                return userType.ToFriendlyString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
