namespace Shared.CustomControls
{
    public partial class BorderOpacityFromBool : Border
    {
        public static readonly BindableProperty SelectedOpacityProperty =
            BindableProperty.Create(nameof(SelectedOpacity), typeof(bool), typeof(BorderOpacityFromBool), propertyChanged: (bindable, oldValu, newValue) =>
            {
                if (bindable is BorderOpacityFromBool view)
                {
                    view.SetOpacity();
                }
            });

        public bool SelectedOpacity
        {
            get => (bool)GetValue(SelectedOpacityProperty);
            set => SetValue(SelectedOpacityProperty, value);
        }

        public static readonly BindableProperty ValueOpacityProperty =
            BindableProperty.Create(nameof(ValueOpacity), typeof(double), typeof(BorderOpacityFromBool), propertyChanged: (bindable, oldValu, newValue) =>
            {
                if (bindable is BorderOpacityFromBool view)
                {
                    view.SetOpacity();
                }
            });

        public double ValueOpacity
        {
            get => (double)GetValue(ValueOpacityProperty);
            set => SetValue(ValueOpacityProperty, value);
        }

        private readonly double defaultOpacity;
        public BorderOpacityFromBool()
        {
            defaultOpacity = this.Opacity;
            ValueOpacity = this.Opacity;
        }
        private void SetOpacity()
        {
            if (SelectedOpacity)
            {
                this.Opacity = ValueOpacity;
            }
            else
            {
                this.Opacity = defaultOpacity;
            }
        }

    }
}
