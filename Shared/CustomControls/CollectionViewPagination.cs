using System.Collections;

namespace Shared.CustomControls
{
    public partial class CollectionViewPagination : CollectionView, IDisposable
    {
        public static readonly BindableProperty ItemsSourcePaginationProperty
            = BindableProperty.Create(nameof(ItemsSourcePagination), typeof(IEnumerable), typeof(CollectionViewPagination), propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is CollectionViewPagination view)
                {
                    view.ItemsSource = null;
                    if (newValue is IEnumerable value)
                    {
                        view._length = 0;
                        foreach (var item in value)
                        {
                            view._length++;
                        }
                        view.LoadCurrent(view._currentPagination, view._currentPage);
                        view._mainGrid.IsVisible = true;
                        if (view._length <= 0 || view._length < int.Parse(view._pickerPagination.Items[0]))
                        {
                            view._mainGrid.IsVisible = false;
                        }
                    }
                }
            });
        public IEnumerable ItemsSourcePagination
        {
            get => (IEnumerable)GetValue(ItemsSourcePaginationProperty);
            set => SetValue(ItemsSourcePaginationProperty, value);
        }

        public static readonly BindableProperty PageSizeOptionsProperty
        = BindableProperty.Create(nameof(PageSizeOptions), typeof(IEnumerable), typeof(int[]), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is CollectionViewPagination view)
            {
                if (newValue is int[] value)
                {
                    view._pickerPagination.ItemsSource.Clear();
                    view._pickerPagination.ItemsSource = value;
                }
            }
        });
        public int[] PageSizeOptions
        {
            get => (int[])GetValue(PageSizeOptionsProperty);
            set => SetValue(PageSizeOptionsProperty, value);
        }


        private Command SetPageCommandUp => new(() =>
        {
            if (_currentPage + 1 <= _length)
            {
                _currentPage++;
                LoadCurrent(_currentPagination, _currentPage);
            }
        });
        private Command SetPageCommandDown => new(() =>
        {
            if (_currentPage - 1 >= 0)
            {
                _currentPage--;
                LoadCurrent(_currentPagination, _currentPage);
            }
        });
        private Command SetPageCommand => new(() =>
        {
            var value = _current.Text;
            if (int.TryParse(value, out int result))
            {
                if (result - 1 >= 0 && result + 1 <= _length)
                {
                    _currentPage = result;
                    LoadCurrent(_currentPagination, result);
                }
            }
        });


        public CollectionViewPagination()
        {
            _pickerPagination.SelectedIndexChanged += _pickerPagination_SelectedIndexChanged;

            _previous.Command = SetPageCommandDown;
            _next.Command = SetPageCommandUp;

            _current.ReturnCommand = SetPageCommand;

            _mainGrid = CreatedPagination();

            this.FooterTemplate = new DataTemplate(() =>
            {
                return _mainGrid;
            });
        }



        public void Dispose()
        {
            _pickerPagination.SelectedIndexChanged -= _pickerPagination_SelectedIndexChanged;
        }
        private void _pickerPagination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(_pickerPagination.SelectedItem.ToString(), out int result))
            {
                _currentPagination = result;
                LoadCurrent(result, _currentPage);
            }
        }


        private readonly Picker _pickerPagination = new()
        {
            ItemsSource = new int[] { 10, 20, 50 },
            SelectedIndex = 0
        };

        private readonly Grid _mainGrid;

        private readonly Label _currentSize = new();

        private readonly Button _previous = new();
        private readonly Entry _current = new();
        private readonly Button _next = new();
        private int _currentPage = 0;
        private int _currentPagination = 10;
        private int _length = 0;

        private void LoadCurrent(int pagination, int page)
        {
            if (ItemsSourcePagination is null)
            {
                return;
            }
            if (_length <= 0)
            {
                return;
            }
            _previous.IsEnabled = true;
            _next.IsEnabled = true;
            int skip = pagination * page;
            if (skip <= 0)
            {
                _previous.IsEnabled = false;
            }
            if (skip + pagination > _length)
            {
                _next.IsEnabled = false;
            }

            _previous.Text = (page - 1).ToString();
            _current.Text = page.ToString();
            _next.Text = (page + 1).ToString();
            _currentSize.Text = $"{skip}-{(skip + pagination < _length ? skip + pagination : _length)} z {_length}";


            this.ItemsSource = SkipTake(ItemsSourcePagination, skip, pagination);

            static IEnumerable SkipTake(IEnumerable source, int skip, int take)
            {
                int n = 0;
                foreach (var item in source)
                {
                    n++;
                    if (n <= skip)
                    {
                        continue;
                    }
                    if (take == 0)
                    {
                        break;
                    }
                    take--;
                    yield return item;
                }
            }
        }

        private Grid CreatedPagination()
        {
            Grid grid = new()
            {
                Padding = new Thickness(5),
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                ColumnSpacing = 10,
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            _currentSize.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            grid.AddWithSpan(_currentSize, 0, 0, 1, 4);
            grid.Add(_pickerPagination, 0, 1);
            grid.Add(_previous, 1, 1);
            grid.Add(_current, 2, 1);
            grid.Add(_next, 3, 1);

            LoadCurrent(_currentPagination, _currentPage);

            return grid;
        }
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                this.FooterTemplate = new DataTemplate(() =>
                {
                    return _mainGrid;
                });
            }
        }

    }
}
