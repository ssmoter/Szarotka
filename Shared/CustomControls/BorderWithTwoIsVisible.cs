namespace  Shared.CustomControls
{
    public class BorderWithTwoIsVisible : Border
    {
        public static readonly BindableProperty IsVisibleOneProperty
            = BindableProperty.Create(nameof(IsVisibleOne), typeof(bool), typeof(BorderWithTwoIsVisible), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is BorderWithTwoIsVisible view)
                {
                    if (newValue is bool value)
                    {
                        if (!view.IsVisibleSecond && !value)
                        {
                            view.IsVisible = false;
                            view.MaximumHeightRequest = 0;
                        }
                        else
                        {
                            view.IsVisible = true;
                            view.MaximumHeightRequest = double.PositiveInfinity;
                        }
                    }
                }
            });
        public bool IsVisibleOne
        {
            get => (bool)GetValue(IsVisibleOneProperty);
            set => SetValue(IsVisibleOneProperty, value);
        }


        public static readonly BindableProperty IsVisibleSecondProperty
            = BindableProperty.Create(nameof(IsVisibleSecond), typeof(bool), typeof(BorderWithTwoIsVisible), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is BorderWithTwoIsVisible view)
                {
                    if (newValue is bool value)
                    {
                        if (!view.IsVisibleOne && !value)
                        {
                            view.IsVisible = false;
                            view.MaximumHeightRequest = 0;
                        }
                        else
                        {
                            view.IsVisible = true;
                            view.MaximumHeightRequest = double.PositiveInfinity;
                        }
                    }
                }
            });
        public bool IsVisibleSecond
        {
            get => (bool)GetValue(IsVisibleSecondProperty);
            set => SetValue(IsVisibleSecondProperty, value);
        }
    }
}
