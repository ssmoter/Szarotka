using System.Globalization;

namespace DataBase.Helper
{
    public static class TranslateDayOfWeekStatic
    {
        public static string TranslateSelectedDay(this DayOfWeek day)
        {
            string name = "";

            switch (day)
            {
                case DayOfWeek.Sunday:
                    name += "Niedziela";
                    break;
                case DayOfWeek.Monday:
                    name += "Poniedziałek";
                    break;
                case DayOfWeek.Tuesday:
                    name += "Wtorek";
                    break;
                case DayOfWeek.Wednesday:
                    name += "Środa";
                    break;
                case DayOfWeek.Thursday:
                    name += "Czwartek";
                    break;
                case DayOfWeek.Friday:
                    name += "Piątek";
                    break;
                case DayOfWeek.Saturday:
                    name += "Sobota";
                    break;
                default:
                    name = "Nie wybrano dnia";
                    break;
            }
            return name;
        }
    }

    public class TranslateDayOfWeek : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DayOfWeek day)
            {
                return day.TranslateSelectedDay();
            }
            return parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


}
