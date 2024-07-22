namespace DataBase.CustomControls
{
    public class BorderWithTwoIsVisible : Border
    {
        public static readonly BindableProperty IsVisibleOnePropertyCustom
            = BindableProperty.Create(nameof(IsVisibleOne), typeof(bool), typeof(BorderWithTwoIsVisible), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is BorderWithTwoIsVisible view)
                {
                    if (newValue is bool value)
                    {
                        if (!view.IsVisibleSecond && !value)
                        {
                            view.IsVisible = false;
                        }
                    }
                }
            });
        public bool IsVisibleOne
        {
            get => (bool)GetValue(IsVisibleOnePropertyCustom);
            set => SetValue(IsVisibleOnePropertyCustom, value);
        }


        public static readonly BindableProperty IsVisibleSecondPropertyCustom
            = BindableProperty.Create(nameof(IsVisibleSecond), typeof(bool), typeof(BorderWithTwoIsVisible), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is BorderWithTwoIsVisible view)
                {
                    if (newValue is bool value)
                    {
                        if (value)
                        {
                            view.BackgroundColor = Colors.Red;
                        }

                        if (!view.IsVisibleOne && !value)
                        {
                            view.IsVisible = false;
                        }
                    }
                }
            });
        public bool IsVisibleSecond
        {
            get => (bool)GetValue(IsVisibleSecondPropertyCustom);
            set => SetValue(IsVisibleSecondPropertyCustom, value);
        }

    }
}
