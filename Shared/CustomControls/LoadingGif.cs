namespace Shared.CustomControls
{
    public partial class LoadingGif : Grid, IDisposable
    {
        private readonly Image _gif = new() { MaximumHeightRequest = 50 };
        public LoadingGif()
        {
            HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            this.Add(new Label()
            {
                Text = "Wczytywanie",
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false)
            }, 0, 3);

            this.Add(_gif, 0, 2);

            void StartRotation()
            {
                _gif.Rotation = 0;
                _gif.Animate("RotateIcon", new Animation(
                    callback: d => _gif.Rotation = d,
                    start: 0,
                    end: 360
                ), length: 1000, easing: Easing.Linear, finished: (v, c) =>
                {
                    if (!c) StartRotation();
                });
            }

            StartRotation();
            Current_RequestedThemeChanged(null, null);
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            _gif.Source = Application.Current.RequestedTheme == AppTheme.Light
                    ? Shared.Helper.Img.FlyoutHeaderCustomContent.OutlineSyncBlack
                    : Shared.Helper.Img.FlyoutHeaderCustomContent.OutlineSyncWhite;
        }

        public void Dispose()
        {
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }
    }
}
