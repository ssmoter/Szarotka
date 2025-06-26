namespace Shared.CustomControls
{
    public partial class BoxViewColorsFromBool : BoxView
    {
        public static readonly BindableProperty SelectedColorProperty =
        BindableProperty.Create(nameof(SelectedColor), typeof(bool), typeof(BoxViewColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BoxViewColorsFromBool view)
            {
                view.SelectOption();
            }

        });

        public bool SelectedColor
        {
            get => (bool)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        public static readonly BindableProperty IsAnimationProperty =
                BindableProperty.Create(nameof(IsAnimation), typeof(bool), typeof(BoxViewColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
                {
                });

        public bool IsAnimation
        {
            get => (bool)GetValue(IsAnimationProperty);
            set => SetValue(IsAnimationProperty, value);
        }
        public static readonly BindableProperty BackgroundColorFirstProperty =
        BindableProperty.Create(nameof(BackgroundColorFirst), typeof(Color), typeof(BoxViewColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BoxViewColorsFromBool view)
            {
                view.SetGradient();
            }
        });

        public Color BackgroundColorFirst
        {
            get => (Color)GetValue(BackgroundColorFirstProperty);
            set => SetValue(BackgroundColorFirstProperty, value);
        }


        public static readonly BindableProperty BackgroundColorSecondProperty =
        BindableProperty.Create(nameof(BackgroundColorSecond), typeof(Color), typeof(BoxViewColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BoxViewColorsFromBool view)
            {
                view.SetGradient();
            }
        });

        public Color BackgroundColorSecond
        {
            get => (Color)GetValue(BackgroundColorSecondProperty);
            set => SetValue(BackgroundColorSecondProperty, value);
        }

        public static readonly BindableProperty IsGradientProperty =
        BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(BoxViewColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BoxViewColorsFromBool view)
            {
            }
        });

        public bool IsGradient
        {
            get => (bool)GetValue(IsGradientProperty);
            set => SetValue(IsGradientProperty, value);
        }


        private void SelectOption()
        {
            if (IsAnimation)
            {
                AnimateGradient();
            }
            if (!IsAnimation)
            {
                SetGradient();
            }
        }

        private void SetGradient()
        {
            this.Background = CreatedGradient();
        }

        private LinearGradientBrush CreatedGradient()
        {
            this.Color = Colors.Transparent;
            return new LinearGradientBrush
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5),
                GradientStops = new GradientStopCollection
                    {
                        new GradientStop
                        {
                            Color = SelectedColor?BackgroundColorFirst:BackgroundColorSecond,
                            Offset = IsGradient ? 0.1f : 1
                        },
                        new GradientStop
                        {
                            Color = Colors.Transparent,
                            Offset = IsGradient ? 0.9f  :0
                        }
                    }
            };
        }



        private void AnimateGradient()
        {
            var gradientBrush = CreatedGradient();

            this.Color = Colors.Transparent;
            this.Background = gradientBrush;

            var stop = gradientBrush.GradientStops[0];

            // Animacja Offset z 0 do 0.1 przez 500ms
            new Animation(
                callback: v => stop.Offset = (float)v,
                start: IsGradient ? 0.0 : 0.0,
                end: IsGradient ? 0.4 : 1
            ).Commit(this, "GradientAnim", length: 500, easing: Easing.CubicInOut);
        }


    }
}
