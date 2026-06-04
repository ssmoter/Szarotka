using System.Windows.Input;

namespace Shared.CustomControls
{
    public class DrawableButton : GraphicsView
    {

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create(nameof(ButtonCommand), typeof(ICommand), typeof(DrawableButton), propertyChanged:  (bindable, oldValu, newValue) =>
            {
            });

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }




        public DrawableButton()
        {
            this.Invalidate();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await this.BounceOnPressAsync();
                ButtonCommand?.Execute(this);
            };


            GestureRecognizers.Add(tapGestureRecognizer);
        }


        TapGestureRecognizer tapGestureRecognizer = new ();

        public async Task<bool> BounceOnPressAsync()
        {
            await this.ScaleToAsync(1.2, 100, Easing.BounceIn);

            return await this.ScaleToAsync(1.0, 100, Easing.BounceOut);
        }

    }
}

