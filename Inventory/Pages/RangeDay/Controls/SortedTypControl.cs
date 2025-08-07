using System.Globalization;
using System.Windows.Input;

namespace Inventory.Pages.RangeDay.Controls
{
    public partial class SortedTypControl : Grid, IDisposable
    {
        public static readonly BindableProperty SortedListProperty
            = BindableProperty.Create(nameof(SortedList), typeof(string[]), typeof(SortedTypControl), propertyChanged: (bindable, oldValue, newValue) =>
            {
            });
        public string[] SortedList
        {
            get => (string[])GetValue(SortedListProperty);
            set => SetValue(SortedListProperty, value);
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create(nameof(ButtonCommand), typeof(ICommand), typeof(SortedTypControl));

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }
        public static readonly BindableProperty ButtonCommandParameterProperty =
             BindableProperty.Create(nameof(ButtonCommandParameter), typeof(object), typeof(SortedTypControl));

        public object ButtonCommandParameter
        {
            get => GetValue(ButtonCommandParameterProperty);
            set => SetValue(ButtonCommandParameterProperty, value);
        }

        private readonly ImageButton _asc = new()
        {

        };
        private readonly ImageButton _desc = new()
        {
            Rotation = 180,
        };
        private readonly Entry _name = new()
        {
        };

        private readonly CollectionView _items = new()
        {
            SelectionMode = SelectionMode.Single,
            ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    Margin = new Thickness(2),
                    HorizontalOptions = LayoutOptions.Start,
                };
                label.SetBinding(Label.TextProperty, ".");

                var grid = new Grid
                {
                    Padding = new Thickness(2),
                    Margin = new Thickness(1),
                };

                grid.Add(label);

                // Zmiana koloru po zaznaczeniu
                grid.SetBinding(VisualElement.BackgroundColorProperty,
                    new Binding("IsSelected", source: grid, converter: new SelectionToColorConverter()));

                return grid;
            }),
        };
        private readonly AbsoluteLayout _absoluteLayout = [];

        public SortedTypControl()
        {
            SortedList = new string[]
            {
                "Jabłko","Gruszka","Śliwka","Jabłko","Gruszka","Śliwka"
            };

            this.AddColumnDefinition(new ColumnDefinition() { Width = new GridLength(50) });
            this.AddColumnDefinition(new ColumnDefinition() { Width = GridLength.Star });
            this.AddColumnDefinition(new ColumnDefinition() { Width = new GridLength(50) });
            this.AddRowDefinition(new RowDefinition() { Height = GridLength.Star });
            this.AddRowDefinition(new RowDefinition() { Height = new GridLength(1) });

            _absoluteLayout.SetLayoutFlags(_absoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
            _absoluteLayout.Add(_items);
            _absoluteLayout.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false);

            _items.MaximumHeightRequest = _name.FontSize * 8;

            this.Add(_asc, 0, 0);
            this.Add(_name, 1, 0);
            this.Add(_desc, 2, 0);
            this.AddWithSpan(_absoluteLayout, 1, 0, 1, 3);
            ZIndex = 1110;
            _name.TextChanged += _name_TextChanged;

            _items.SelectionChanged += (s, e) =>
            {
                string selected = e.CurrentSelection[0]?.ToString();
                _name.Text = selected;
            };


            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            Current_RequestedThemeChanged(null, null);
        }
        public void Dispose()
        {
            _name.TextChanged -= _name_TextChanged;
            Application.Current.RequestedThemeChanged -= Current_RequestedThemeChanged;
        }
        private void _name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (entry.Text.Length > 0)
                {
                    _items.ItemsSource = SortedList.Where(x => x.Contains(entry.Text, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    _items.ItemsSource = null;
                }
            }
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            _asc.Source = Application.Current.RequestedTheme == AppTheme.Light
                    ? Shared.Helper.Img.ImgArrow.ArrowUpwardBlack
                    : Shared.Helper.Img.ImgArrow.ArrowUpwardWhite;
            _desc.Source = Application.Current.RequestedTheme == AppTheme.Light
                    ? Shared.Helper.Img.ImgArrow.ArrowUpwardBlack
                    : Shared.Helper.Img.ImgArrow.ArrowUpwardWhite;

            var color = Application.Current.RequestedTheme == AppTheme.Light
                ? "White" : "OffBlack";
            if (Application.Current.Resources.TryGetValue(color, out var value1) && value1 is Color background)
            {
                _items.Background = background;
            }

        }
        public partial class SelectionToColorConverter : IValueConverter, IDisposable
        {
            public SelectionToColorConverter()
            {
                Current_RequestedThemeChanged(null, null);
                Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            }

            private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
            {

                string colorB;
                string colorS;
                if (Application.Current.RequestedTheme == AppTheme.Light)
                {
                    colorB = "White";
                    colorS = "Gray950";
                }
                else
                {
                    colorB = "OffBlack";
                    colorS = "Gray100";
                }
                if (Application.Current.Resources.TryGetValue(colorB, out var value1) && value1 is Color background)
                {
                    ColorB = background;
                }
                if (Application.Current.Resources.TryGetValue(colorS, out var value2) && value2 is Color selected)
                {
                    ColorS = selected;
                }
            }

            private Color ColorB;
            private Color ColorS;
            public void Dispose()
            {
                Application.Current.RequestedThemeChanged -= Current_RequestedThemeChanged;
            }
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? ColorS : ColorB;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
                throw new NotImplementedException();


        }


    }
}
