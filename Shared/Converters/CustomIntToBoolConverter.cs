using System.Globalization;

namespace Shared.Converters
{
    public class CustomIntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int input)
            {
                var result = false;
                if (input >= 0)
                {
                    result = true;
                }
                if (parameter is bool b)
                {
                    if (b)
                    {
                        result = !result;
                    }
                }
                return result;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
