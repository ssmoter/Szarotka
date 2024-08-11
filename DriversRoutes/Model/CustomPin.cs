using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Model
{
    public class CustomPin : Pin
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CustomPin), defaultBindingMode: BindingMode.TwoWay);
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

    }
}
