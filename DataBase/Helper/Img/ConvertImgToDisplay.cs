using System.Globalization;
namespace DataBase.Helper.Img
{
    public class ConvertImgToDisplay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imgString)
            {
                var imageBytes = System.Convert.FromBase64String(imgString);
                using MemoryStream imageStream = new MemoryStream(imageBytes);
                //  var image = ImageSource.FromStream(() => imageStream);
                var image = new Image() { Source = imgString };
                return image;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
