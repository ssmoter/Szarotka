using Shared.Helper;

namespace Shared.CustomControls.FromCode
{
    public class UpdateProgressBar : IDisposable
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


        public static UpdateProgressBar CreatedUpdateProgressBar(string title = "",
            string description = "", string icon = "", Action action = null)
        {
            var bar = CreatedUpdateProgressBar();

            bar.Title = title;
            bar.Description = description;
            bar.Icon = icon;
            bar.Action = action;

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


            bar._title.FontSize = bar._title.FontSize * 2;
            bar._title.Style = (Style)Application.Current.Resources["LabelPointerOver"];

            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Auto)));
            bar.Grid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Auto)));

            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Auto)));
            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            bar.Grid.AddColumnDefinition(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            bar.Grid.SizeChanged += SizeChange;

            bar.Grid.AddWithSpan(bar.ProgressBar, 0, 0, 1, 3);
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

        public void Dispose()
        {
            Grid.SizeChanged -= SizeChange;
        }
    }
}
