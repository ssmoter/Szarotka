namespace DataBase.CustomControls
{
    public class ButtonColorsFromBool : Button
    {
        public static readonly BindableProperty SelectedColorProperty =
        BindableProperty.Create(nameof(SelectedColor), typeof(bool), typeof(ButtonColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is ButtonColorsFromBool view)
            {
                if (newValue is bool value)
                {
                    if (value)
                    {
                        view.BackgroundColor = view.BackgroundColorFirst;
                    }
                    if (!value)
                    {
                        view.BackgroundColor = view.BackgroundColorSecond;
                    }
                }
            }

        });

        public bool SelectedColor
        {
            get => (bool)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly BindableProperty BackgroundColorFirstProperty =
        BindableProperty.Create(nameof(BackgroundColorFirst), typeof(Color), typeof(ButtonColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is ButtonColorsFromBool view)
            {
                if (newValue is Color value)
                {

                    view.BackgroundColor = value;
                }
            }
        });

        public Color BackgroundColorFirst
        {
            get => (Color)GetValue(BackgroundColorFirstProperty);
            set => SetValue(BackgroundColorFirstProperty, value);
        }


        public static readonly BindableProperty BackgroundColorSecondProperty =
        BindableProperty.Create(nameof(BackgroundColorSecond), typeof(Color), typeof(ButtonColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is ButtonColorsFromBool view)
            {
                if (newValue is Color value)
                {

                    view.BackgroundColor = value;
                }
            }
        });

        public Color BackgroundColorSecond
        {
            get => (Color)GetValue(BackgroundColorSecondProperty);
            set => SetValue(BackgroundColorSecondProperty, value);
        }
    }
}
