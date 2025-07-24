namespace Shared.CustomControls
{
    public partial class BorderColorsFromBool : Border
    {
        public static readonly BindableProperty SelectedColorProperty =
        BindableProperty.Create(nameof(SelectedColor), typeof(bool), typeof(BorderColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BorderColorsFromBool view)
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
                BindableProperty.Create(nameof(IsAnimation), typeof(bool), typeof(BorderColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
                {
                });

        public bool IsAnimation
        {
            get => (bool)GetValue(IsAnimationProperty);
            set => SetValue(IsAnimationProperty, value);
        }
        public static readonly BindableProperty BackgroundColorFirstProperty =
        BindableProperty.Create(nameof(BackgroundColorFirst), typeof(Color), typeof(BorderColorsFromBool), defaultValue: SetDefault(), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BorderColorsFromBool view)
            {
                view.SelectOption();
            }
        });

        public Color BackgroundColorFirst
        {
            get => (Color)GetValue(BackgroundColorFirstProperty);
            set => SetValue(BackgroundColorFirstProperty, value);
        }


        public static readonly BindableProperty BackgroundColorSecondProperty =
        BindableProperty.Create(nameof(BackgroundColorSecond), typeof(Color), typeof(BorderColorsFromBool), defaultValue: SetDefault(), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BorderColorsFromBool view)
            {
                view.SelectOption();
            }
        });

        public Color BackgroundColorSecond
        {
            get => (Color)GetValue(BackgroundColorSecondProperty);
            set => SetValue(BackgroundColorSecondProperty, value);
        }

        public static readonly BindableProperty IsGradientProperty =
        BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(BorderColorsFromBool), propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is BorderColorsFromBool view)
            {
                view.SelectOption();
            }
        });

        public bool IsGradient
        {
            get => (bool)GetValue(IsGradientProperty);
            set => SetValue(IsGradientProperty, value);
        }

        public BorderColorsFromBool()
        {
            SelectOption();

            this.Stroke = SetDefault();
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

            this.Background = gradientBrush;

            var stop = gradientBrush.GradientStops[0];

            // Animacja Offset z 0 do 0.1 przez 500ms
            new Animation(
                callback: v => stop.Offset = (float)v,
                start: IsGradient ? 0.0 : 0.0,
                end: IsGradient ? 0.4 : 1
            ).Commit(this, "GradientAnim", length: 500, easing: Easing.CubicInOut);
        }

        static private Color SetDefault()
        {
            Color grayColor200 = Colors.Gray;
            Color grayColor500 = Colors.Gray;
            if (Application.Current.Resources.TryGetValue("Gray200", out var value2) && value2 is Color grayColor2)
            {
                grayColor200 = grayColor2;
            }
            if (Application.Current.Resources.TryGetValue("Gray500", out var value5) && value5 is Color grayColor5)
            {
                grayColor500 = grayColor5;
            }

            return Application.Current.RequestedTheme != AppTheme.Light
                    ? grayColor500
                    : grayColor200;
        }

    }
}
