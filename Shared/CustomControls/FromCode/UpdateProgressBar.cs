using Shared.Helper;

namespace Shared.CustomControls.FromCode
{
    public partial class UpdateProgressBar : IDisposable
    {
        public ProgressBar ProgressBar { get; set; } = new();
        public Grid Grid { get; set; } = [];
        public string Title
        {
            get => _title.Text;
            set => _title.Text = value;
        }
        public string Description
        {
            get => _description.Text;
            set => _description.Text = value;
        }
        public string Icon
        {
            set => _image.Source = value;
        }

        public Action Action { get; set; }


        private readonly Label _title = new();
        private readonly Label _description = new();
        private readonly Image _image = new() { Source = "logo2.png" };
        private readonly Label _percent = new()
        {
            HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false)
        };

        public static UpdateProgressBar CreatedUpdateProgressBar(string title = "",
            string description = "", string icon = "", bool rotateIcon = false, Action action = null)
        {
            var bar = CreatedUpdateProgressBar();

            bar.Title = title;
            bar.Description = description;
            bar.Icon = icon;
            bar.Action = action;

            if (rotateIcon)
            {
                void StartRotation()
                {
                    bar?._image?.CancelAnimations();
                    bar._image.Rotation = 0;
                    bar._image.Animate("RotateIcon", new Animation(
                        callback: d => bar._image.Rotation = d,
                        start: 0,
                        end: 360
                    ), length: 1000, easing: Easing.Linear, finished: (v, c) =>
                    {
                        if (!c) StartRotation();
                    });
                }
                StartRotation();
            }

            return bar;
        }
        public static UpdateProgressBar CreatedUpdateProgressBar()
        {
            var bar = new UpdateProgressBar();

            bar._title.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Task.Run(async () =>
                    {
                        await bar._title.BounceOnPressAsync();
                    });
                    bar.Action?.Invoke();
                })
            });


            bar._title.FontSize *= 2;
            bar._title.Style = (Style)Application.Current.Resources["LabelPointerOver"];

            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Auto)));
            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Auto)));

            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Auto)));
            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            bar.Grid.SizeChanged += SizeChange;

            bar.Grid.AddWithSpan(bar.ProgressBar, 0, 0, 1, 3);
            bar.Grid.AddWithSpan(bar._percent, 0, 0, 1, 3);
            bar.Grid.AddWithSpan(bar._image, 1, 0, 2, 1);
            bar.Grid.AddWithSpan(bar._title, 1, 1, 1, 2);
            bar.Grid.AddWithSpan(bar._description, 2, 1, 1, 2);

            return bar;
        }

        private static void SizeChange(object sender, EventArgs e)
        {
            var grid = sender as Grid;

            var oneThird = grid.DesiredSize.Width / 3;
            var _image = grid.Children.OfType<Image>().FirstOrDefault();

            if (_image.MaximumWidthRequest != oneThird)
            {
                _image.MaximumWidthRequest = oneThird;
            }
        }

        public static void UpdateProgress(UpdateProgressBar updateProgressBar, double progress)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (updateProgressBar is not null)
                {
                    //bar.Progress = progress;
                    updateProgressBar._percent.Text = GetOnly4(progress);
                    await updateProgressBar.ProgressBar.ProgressTo(progress, 500, Easing.Linear);
                }
            });
            static string GetOnly4(double progress)
            {
                if (progress > 0 && progress < 1)
                {
                    Span<char> buffer = stackalloc char[32];
                    bool success = progress.TryFormat(buffer, out int charsWritten);
                    ReadOnlySpan<char> span = buffer[..charsWritten];
                    return span[..4].ToString();
                }
                return progress.ToString();
            }
        }

        public static string GetSyncImage()
        {
            return Application.Current.RequestedTheme == AppTheme.Light
                    ? Shared.Helper.Img.FlyoutHeaderCustomContent.OutlineSyncBlack
                    : Shared.Helper.Img.FlyoutHeaderCustomContent.OutlineSyncWhite;
        }

        public void Dispose()
        {
            Grid.SizeChanged -= SizeChange;
            Action = null;
            GC.SuppressFinalize(this);
        }
    }
}
