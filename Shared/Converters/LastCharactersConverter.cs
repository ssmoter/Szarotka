using System.Globalization;

namespace Shared.Converters
{
    public class LastCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            if (value is Guid)
            {
                input = value.ToString();
            }
            if (input != null)
            {
                if (int.TryParse(parameter as string, out int numberOfCharacters) && numberOfCharacters > 0)
                {
                    return input.Length > numberOfCharacters
                        ? string.Concat("...", input.AsSpan(input.Length - numberOfCharacters))
                        : input;
                }
            }
            return input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
