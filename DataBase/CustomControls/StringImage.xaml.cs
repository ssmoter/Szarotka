namespace DataBase.CustomControls;

public partial class StringImage : ContentView
{
    public static readonly BindableProperty String64BaseProperty
        = BindableProperty.Create(nameof(String64Base), typeof(string), typeof(StringImage), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (StringImage)bindable;
            try
            {
                if(string.IsNullOrWhiteSpace((string)newValue))
                    throw new Exception();

                var imageBytes = System.Convert.FromBase64String((string)newValue);
                
                if (imageBytes is null)
                    throw new Exception();

                var imageStream = new MemoryStream(imageBytes);
                var image = ImageSource.FromStream(() => imageStream);
                control.ImageFromString.Source = image;
            }
            catch (FormatException)
            {
                control.ImageFromString.Source = ImageSource.FromFile((string)newValue);
            }
            catch (Exception)
            {
                control.ImageFromString.Source = ImageSource.FromFile(DataBase.Helper.Img.ImgPath.Logo);
            }

        });
    public string String64Base
    {
        get => GetValue(String64BaseProperty) as string;
        set => SetValue(String64BaseProperty, value);
    }

    public static readonly BindableProperty HeightRequestPropertyCustom
    = BindableProperty.Create(nameof(HeightRequestCustom), typeof(string), typeof(StringImage), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StringImage)bindable;

        if (double.TryParse((string)newValue, out double value))
        {
            control.ImageFromString.HeightRequest = value;
        }
    });
    public string HeightRequestCustom
    {
        get => GetValue(HeightRequestPropertyCustom) as string;
        set => SetValue(HeightRequestPropertyCustom, value);
    }

    public static readonly BindableProperty WidthRequestPropertyCustom
= BindableProperty.Create(nameof(WidthRequestCustom), typeof(string), typeof(StringImage), propertyChanged: (bindable, oldValue, newValue) =>
{
    var control = (StringImage)bindable;

    if (double.TryParse((string)newValue, out double value))
    {
        control.ImageFromString.WidthRequest = value;
    }
});
    public string WidthRequestCustom
    {
        get => GetValue(WidthRequestPropertyCustom) as string;
        set => SetValue(WidthRequestPropertyCustom, value);
    }

    public StringImage()
    {
        InitializeComponent();
    }



}