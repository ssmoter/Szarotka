namespace DataBase.CustomControls
{
    public class MovingViewInSteps : ContentView, IDisposable
    {
        public static readonly BindableProperty StepStartProperty =
            BindableProperty.Create(nameof(StepStart), typeof(StepSelected), typeof(MovingViewInSteps), defaultValue: StepSelected.One, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
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
             BindableProperty.Create(nameof(Header), typeof(IView), typeof(MovingViewInSteps), propertyChanged: (bindable, oldValue, newValue) =>
             {
                 if (bindable is MovingViewInSteps view)
                 {
                     if (newValue is not null)
                     {
                         view._contenGrid.Remove(oldValue as IView);
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
             BindableProperty.Create(nameof(ContentView), typeof(IView), typeof(MovingViewInSteps), propertyChanged: (bindable, oldValue, newValue) =>
             {
                 if (bindable is MovingViewInSteps view)
                 {
                     if (newValue is View value)
                     {
                         view._contenGrid.Remove(value);
                         int direction = (view.Direction == Direction.Up) ? 1 : 0;
                         view._contenGrid.Add(value, column: 0, row: direction);
                     }
                 }
             });
        public View ContentView
        {
            get => (View)GetValue(ContentViewProperty);
            set => SetValue(ContentViewProperty, value);
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create(nameof(Direction), typeof(Direction), typeof(MovingViewInSteps), defaultValue: Direction.Up, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
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
        private event EventHandler<PanUpdatedEventArgs> _panUpdated;
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

            //GestureRecognizers.Add(GetSwipeGestureRecognizerUp());
            //GestureRecognizers.Add(GetSwipeGestureRecognizerDown());
            GestureRecognizers.Add(GetPanGestureRecognizer());
        }

        public void Dispose()
        {
            GestureRecognizers.Clear();
            _swipeUp -= MovingViewInStepsUp_Swipe;
            _swipeDown -= MovingViewInStepsDown_Swipe;
            _animation.Dispose();
        }


        private PanGestureRecognizer GetPanGestureRecognizer()
        {
            PanGestureRecognizer pan = new();
            pan.PanUpdated += Pan_PanUpdated;
            return pan;
        }

        private double initialHeight;
        private double startY;
        StepSelected newStep = StepSelected.None;
        private void Pan_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var parent = (this.Parent as View);
            var height = parent.Height;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        // initialWidth = Width;
                        initialHeight = Height;
                        //  startX = e.TotalX;
                        startY = e.TotalY;
                    }
                    break;
                case GestureStatus.Running:
                    {
                        double newHeight = 0;

                        if (Direction == Direction.Up)
                        {
                            newHeight = initialHeight + (-e.TotalY - startY);
                        }
                        if (Direction == Direction.Down)
                        {
                            newHeight = initialHeight + (e.TotalY - startY);
                        }

                        if (newHeight < 0
                            || newHeight > height)
                        {
                            break;
                        }
                       // this.ContentView.HeightRequest = newHeight;
                        //_animation = [];
                        _animation.Add(0, 1, new Animation(v => this.ContentView.HeightRequest = v, this.ContentView.Height, newHeight));
                        _animation.Commit(this, "GrowAnimation", 16, 250, Easing.Linear);

                        var minimHeight = height / (int)StepSelected.Full;

                        if (newHeight < minimHeight)
                            newStep = StepSelected.None;
                        if (newHeight > minimHeight && newHeight < 2 * minimHeight)
                            newStep = StepSelected.One;
                        if (newHeight > 2 * minimHeight)
                            newStep = StepSelected.Full;
                    }
                    break;
                case GestureStatus.Completed:
                    {
                        if (newStep >= StepSelected.None && newStep <= StepSelected.Full)
                        {
                            StepStart = StepSelected.None;
                            StepStart = newStep;
                        }
                    }
                    break;
            }


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
            var parent = (this.Parent as View);
            var height = parent.Height;

            var newHeight = (double)((height / 3) * (int)step);
            if (step != StepSelected.None)
            {
                ContentView.IsVisible = true;
            }
            if (step == StepSelected.None)
            {
                newHeight = 1;
            }

            _animation.Add(0, 1, new Animation(v => this.ContentView.HeightRequest = v, this.ContentView.Height, newHeight));
            _animation.Commit(this, "GrowAnimation", 16, 250, Easing.Linear);

            if (step == StepSelected.None)
            {
                ContentView.IsVisible = false;
            }

        }
        public static StepSelected StepUp(StepSelected step)
        {
            var newStep = ((int)step) + 1;

            if (newStep > (int)StepSelected.Full)
            {
                return step;
            }

            return (StepSelected)newStep;
        }
        public static StepSelected StepDown(StepSelected step)
        {
            var newStep = ((int)step) - 1;

            if (newStep < (int)StepSelected.None)
            {
                return step;
            }

            return (StepSelected)newStep;
        }
        private static void SetDirection(MovingViewInSteps mainView, Direction direction)
        {
            mainView._swipeUp -= mainView.MovingViewInStepsUp_Swipe;
            mainView._swipeDown -= mainView.MovingViewInStepsDown_Swipe;
            mainView._contenGrid.Remove(mainView.Header);
            mainView._contenGrid.Remove(mainView.ContentView);

            if (direction == Direction.Up)
            {
                if (mainView.Header is not null)
                {
                    mainView._contenGrid.Add(mainView.Header, column: 0, row: 0);
                }
                if (mainView.ContentView is not null)
                {
                    mainView._contenGrid.Add(mainView.ContentView, column: 0, row: 1);
                }
                mainView.VerticalOptions = LayoutOptions.End;
                mainView._swipeUp += mainView.MovingViewInStepsUp_Swipe;
                mainView._swipeDown += mainView.MovingViewInStepsDown_Swipe;
            }
            else if (direction == Direction.Down)
            {
                if (mainView.Header is not null)
                {
                    mainView._contenGrid.Add(mainView.Header, column: 0, row: 1);
                }
                if (mainView.ContentView is not null)
                {
                    mainView._contenGrid.Add(mainView.ContentView, column: 0, row: 0);
                }
                mainView.VerticalOptions = LayoutOptions.Start;
                mainView._swipeUp += mainView.MovingViewInStepsDown_Swipe;
                mainView._swipeDown += mainView.MovingViewInStepsUp_Swipe;
            }
        }
    }

    public enum StepSelected
    {
        None = 0,
        One,
        Half,
        Full,
    }
    public enum Direction
    {
        Up,
        Down,
    }
}
