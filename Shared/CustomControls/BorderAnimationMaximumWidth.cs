namespace Shared.CustomControls
{
    public partial class BorderAnimationMaximumWidth : Border
    {
        public static readonly BindableProperty AnimationWidthProperty
            = BindableProperty.Create(nameof(AnimationWidth),
                typeof(bool),
                typeof(BorderAnimationMaximumWidth),
                defaultBindingMode: BindingMode.TwoWay,
                defaultValue: false,
                propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is BorderAnimationMaximumWidth view)
                {
                    if (newValue is bool value)
                    {
                        if (value == true)
                        {
                            view.StartAnimation();
                        }
                        else
                        {
                            view.StopAnimation();
                        }
                    }
                }
            });

        public bool AnimationWidth
        {
            get => (bool)GetValue(AnimationWidthProperty);
            set => SetValue(AnimationWidthProperty, value);
        }

        private Animation _animation = [];
        public BorderAnimationMaximumWidth()
        {
            StopAnimation();
        }
        void StartAnimation()
        {
            var width = GetMaxWidth();

            _animation.Add(0, 1, new Animation(v => this.WidthRequest = v, this.Width, width));
            _animation.Commit(this, "GrowAnimation", 16, 250, Easing.Linear);
        }
        void StopAnimation()
        {
            var width = 0;
            _animation.Add(0, 1, new Animation(v => this.WidthRequest = v, this.Width, width));
            _animation.Commit(this, "GrowAnimation", 16, 250, Easing.Linear);
        }

        double GetMaxWidth()
        {
            double max = 0;

            var parent = this.Parent as View;
            if (parent is null)
            {
                return max;
            }

            var bounds = parent.Bounds;

            max = bounds.Width;

            return max;
        }
    }
}
