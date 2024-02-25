using DriversRoutes.Model;

using System.Globalization;

namespace DriversRoutes.Helper
{
    public class DisplayDayAndHourFromOneWeek : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SelectedDayOfWeekRoutes day)
            {
                return day.ToStringWithTheTime();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
