
namespace DataBase.CustomControls
{
    public class MovingViewInSteps : ContentView, IDisposable
    {
        public static readonly BindableProperty StepStartProperty =
            BindableProperty.Create(nameof(StepStart), typeof(StepSelected), typeof(MovingViewInSteps), defaultValue: StepSelected.One, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValu, newValue) =>
            {
                if (bindable is MovingViewInSteps view)
                {
                    if (newValue is StepSelected value)
                    {
                        view.ChangeContentSize(value);
                    }
                }
            });
        public StepSelected StepStart
        {
            get => (StepSelected)GetValue(StepStartProperty);
            set => SetValue(StepStartProperty, value);
        }

        public static readonly BindableProperty HeaderProperty =
             BindableProperty.Create(nameof(Header), typeof(IView), typeof(MovingViewInSteps), propertyChanged: (bindable, oldValu, newValue) =>
             {
                 if (bindable is MovingViewInSteps view)
                 {
                     if (newValue is not null)
                     {
                         view._contenGrid.Remove(oldValu as IView);
                         int direction = (view.Direction == Direction.Up) ? 0 : 1;
                         view._contenGrid.Add(newValue as IView, column: 0, row: direction);
                     }
                 }
             });
        public IView Header
        {
            get => (IView)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly BindableProperty ContentViewProperty =
             BindableProperty.Create(nameof(ContentView), typeof(IView), typeof(MovingViewInSteps), propertyChanged: (bindable, oldValu, newValue) =>
             {
                 if (bindable is MovingViewInSteps view)
                 {
                     if (newValue is not null)
                     {
                         view._contenGrid.Remove(oldValu as IView);
                         int direction = (view.Direction == Direction.Up) ? 1 : 0;
                         view._contenGrid.Add(newValue as IView, column: 0, row: direction);
                     }
                 }
             });
        public View ContentView
        {
            get => (View)GetValue(ContentViewProperty);
            set => SetValue(ContentViewProperty, value);
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create(nameof(Direction), typeof(Direction), typeof(MovingViewInSteps), defaultValue: Direction.Up, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValu, newValue) =>
            {
                if (bindable is MovingViewInSteps view)
                {
                    if (newValue is Direction value)
                    {
                        SetDirection(view, value);
                    }
                }
            });
        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }



        private Animation _animation = [];
        private event EventHandler<SwipedEventArgs> _swipeUp;
        private event EventHandler<SwipedEventArgs> _swipeDown;
        private Grid _contenGrid => this.Content as Grid;
        public MovingViewInSteps()
        {
            var grid = new Grid();
            grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            Content = grid;

            _swipeUp += MovingViewInStepsUp_Swipe;
            _swipeDown += MovingViewInStepsDown_Swipe;
            VerticalOptions = LayoutOptions.End;

            GestureRecognizers.Add(GetSwipeGestureRecognizerUp());
            GestureRecognizers.Add(GetSwipeGestureRecognizerDown());
        }
        public void Dispose()
        {
            GestureRecognizers.Clear();
            _swipeUp -= MovingViewInStepsUp_Swipe;
            _swipeDown -= MovingViewInStepsDown_Swipe;
            _animation.Dispose();
        }

        private SwipeGestureRecognizer GetSwipeGestureRecognizerUp()
        {
            SwipeGestureRecognizer swipeUp = new()
            {
                Direction = SwipeDirection.Up,
            };
            swipeUp.Swiped += (sender, e) => _swipeUp?.Invoke(this, e);
            return swipeUp;
        }
        private SwipeGestureRecognizer GetSwipeGestureRecognizerDown()
        {
            SwipeGestureRecognizer swipeUp = new()
            {
                Direction = SwipeDirection.Down,
            };
            swipeUp.Swiped += (sender, e) => _swipeDown?.Invoke(this, e);
            return swipeUp;
        }
        private void MovingViewInStepsUp_Swipe(object sender, SwipedEventArgs e)
        {
            StepStart = StepUp(StepStart);
            ChangeContentSize(StepStart);
        }
        private void MovingViewInStepsDown_Swipe(object sender, SwipedEventArgs e)
        {
            StepStart = StepDown(StepStart);
            ChangeContentSize(StepStart);
        }
        private void ChangeContentSize(StepSelected step)
        {
            if (ContentView is null)
            {
                return;
            }
            var height = Application.Current?.Windows[0]?.Height;

            var newHeight = (double)((height / 4) * (int)step);

            _animation.Add(0, 1, new Animation(v => this.ContentView.MaximumHeightRequest = v, this.ContentView.Height, newHeight));
            _animation.Commit(this, "GrowAnimation", 16, 1000, Easing.Linear);
        }
        private static StepSelected StepIsChange(StepSelected step, bool goUp)
        {
            if (goUp)
            {
                step = StepUp(step);
            }
            else
            {
                step = StepDown(step);
            }
            return step;
        }
        private static StepSelected StepUp(StepSelected step)
        {
            var newStep = ((int)step) + 1;

            if (newStep > (int)StepSelected.Full)
            {
                return step;
            }

            return (StepSelected)newStep;
        }
        private static StepSelected StepDown(StepSelected step)
        {
            var newStep = ((int)step) - 1;

            if (newStep < (int)StepSelected.None)
            {
                return step;
            }

            return (StepSelected)newStep;
        }
        private static void SetDirection(MovingViewInSteps view, Direction direction)
        {
            view._swipeUp -= view.MovingViewInStepsUp_Swipe;
            view._swipeDown -= view.MovingViewInStepsDown_Swipe;
            view._contenGrid.Remove(view.Header);
            view._contenGrid.Remove(view.ContentView);

            if (direction == Direction.Up)
            {
                if (view.Header is not null)
                {
                    view._contenGrid.Add(view.Header, column: 0, row: 0);
                }
                if (view.ContentView is not null)
                {
                    view._contenGrid.Add(view.ContentView, column: 0, row: 1);
                }
                view.VerticalOptions = LayoutOptions.End;
                view._swipeUp += view.MovingViewInStepsUp_Swipe;
                view._swipeDown += view.MovingViewInStepsDown_Swipe;
            }
            else if (direction == Direction.Down)
            {
                if (view.Header is not null)
                {
                    view._contenGrid.Add(view.Header, column: 0, row: 1);
                }
                if (view.ContentView is not null)
                {
                    view._contenGrid.Add(view.ContentView, column: 0, row: 0);
                }
                view.VerticalOptions = LayoutOptions.Start;
                view._swipeUp += view.MovingViewInStepsDown_Swipe;
                view._swipeDown += view.MovingViewInStepsUp_Swipe;
            }
        }
    }

    public enum StepSelected
    {
        None = 0,
        One,
        Half,
        Three,
        Full,
    }
    public enum Direction
    {
        Up,
        Down,
    }
}
