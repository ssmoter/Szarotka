
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Inventory.Pages.RangeDay.Controls
{
    public partial class HeaderFromList : Grid
    {
        public static readonly BindableProperty HeadersProperty
        = BindableProperty.Create(nameof(Headers), typeof(ObservableCollection<string>), typeof(HeaderFromList), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is HeaderFromList view)
            {
                if (oldValue is ObservableCollection<string> oldCollection)
                    oldCollection.CollectionChanged -= view.OnCollectionChanged;

                if (newValue is ObservableCollection<string> result)
                {
                    result.CollectionChanged += view.OnCollectionChanged;
                    CreatedHeader(view, view.HeaderCommand, result);
                }
            }
        });
        public ObservableCollection<string> Headers
        {
            get => (ObservableCollection<string>)GetValue(HeadersProperty);
            set => SetValue(HeadersProperty, value);
        }
        public static readonly BindableProperty HeaderCommandProperty =
       BindableProperty.Create(nameof(HeaderCommand), typeof(ICommand), typeof(HeaderFromList));

        public ICommand HeaderCommand
        {
            get => (ICommand)GetValue(HeaderCommandProperty);
            set => SetValue(HeaderCommandProperty, value);
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CreatedHeader(this, HeaderCommand, Headers);
        }
        static private void CreatedHeader(Grid grid, ICommand command, IEnumerable<string> inputs)
        {
            int row = -1;
            grid.Clear();
            grid.ClearLogicalChildren();
            grid.RowDefinitions.Clear();
            grid.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false);
            foreach (var item in inputs)
            {
                row++;
                if (item == "*")
                {
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(30, GridUnitType.Absolute) });
                }
                else
                {
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
                grid.Add(MainBorder(GetLabel(item), command), 0, row);
                grid.Add(GetBorder(), 0, row);
            }

            if (row == -1)
            {
                return;
            }
            grid.AddWithSpan(
                new Border()
                {
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.End, false)
                }
                , 0, 0, row + 1, 1);
        }
        private static Image lastImage;
        private static string lastLabel = "";
        private static FilterTyp.OrderTyp orderTyp = FilterTyp.OrderTyp.None;
        private static bool _isScheduledOrderBy = false;
        private static Border MainBorder(Label label, ICommand command)
        {
            var border = new Border()
            {
                StrokeThickness = 0,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
            };
            var image = new Image();
            image.SetAppTheme(Image.SourceProperty, Shared.Helper.Img.ImgFilter.OutlineFilterListBlack, Shared.Helper.Img.ImgFilter.OutlineFilterListWhite);
            TapGestureRecognizer tap = new()
            {
                Command = command,
                CommandParameter = label.Text
            };
            tap.Tapped += async (s, e) =>
            {

                if (lastLabel != label.Text)
                {
                    orderTyp = FilterTyp.OrderTyp.None;
                    _isScheduledOrderBy = false;
                }
                if (_isScheduledOrderBy)
                    return;

                _isScheduledOrderBy = true;
                await Task.Delay(FilterTyp.TimerDelay);

                if (orderTyp == FilterTyp.OrderTyp.Desc)
                {
                    orderTyp = FilterTyp.OrderTyp.Asc;
                }
                else if (orderTyp == FilterTyp.OrderTyp.Asc)
                {
                    orderTyp = FilterTyp.OrderTyp.Desc;
                }
                else
                {
                    orderTyp = FilterTyp.OrderTyp.Asc;
                }
                if (lastImage is not null)
                {
                    lastImage.Rotation = 0;
                    lastImage?.SetAppTheme(Image.SourceProperty, Shared.Helper.Img.ImgFilter.OutlineFilterListBlack, Shared.Helper.Img.ImgFilter.OutlineFilterListWhite);
                }

                switch (orderTyp)
                {
                    case FilterTyp.OrderTyp.None:
                        image?.SetAppTheme(Image.SourceProperty, Shared.Helper.Img.ImgFilter.OutlineFilterListBlack, Shared.Helper.Img.ImgFilter.OutlineFilterListWhite);
                        image.Rotation = 0;
                        break;
                    case FilterTyp.OrderTyp.Desc:
                        image?.SetAppTheme(Image.SourceProperty, Shared.Helper.Img.ImgFilter.OutlineSortBlack, Shared.Helper.Img.ImgFilter.OutlineSortWhite);
                        image.Rotation = 0;
                        break;
                    case FilterTyp.OrderTyp.Asc:
                        image?.SetAppTheme(Image.SourceProperty, Shared.Helper.Img.ImgFilter.OutlineSortBlack, Shared.Helper.Img.ImgFilter.OutlineSortWhite);
                        image.Rotation = 180;
                        break;
                }
                lastLabel = label.Text;
                lastImage = image;
                _isScheduledOrderBy = false;
            };



            border.GestureRecognizers.Add(tap);

            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });



            grid.Add(image, 0, 1);
            grid.Add(label, 0, 0);

            border.Content = grid;

            return border;
        }

        private static Label GetLabel(string header)
        {
            var label = new Label
            {
                Text = header,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.CharacterWrap,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                HorizontalTextAlignment = TextAlignment.Center
            };
            return label;
        }
        private static Border GetBorder()
        {
            var border = new Border()
            {
                VerticalOptions = new LayoutOptions(LayoutAlignment.End, false),
            };

            return border;
        }

    }
}
