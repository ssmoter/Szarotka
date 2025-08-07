using DataBase.Model.EntitiesInventory;

using Shared.Helper;


namespace Inventory.Pages.RangeDay.Controls
{
    public partial class ContentFromList : Grid
    {
        public static readonly BindableProperty DayExpandedProperty
        = BindableProperty.Create(nameof(DayExpanded), typeof(DayExpanded), typeof(ContentFromList), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is ContentFromList view)
            {
                if (newValue is DayExpanded result)
                {
                    CreatedContent(view, result);
                }
            }
        });
        public DayExpanded DayExpanded
        {
            get => (DayExpanded)GetValue(DayExpandedProperty);
            set => SetValue(DayExpandedProperty, value);
        }




        static private void CreatedContent(Grid grid, DayExpanded inputs)
        {
            int row = -1;
            grid.Clear();
            grid.ClearLogicalChildren();
            grid.RowDefinitions.Clear();
            grid.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false);
            foreach (var item in inputs.SelectedHeaders)
            {
                var itemLower = item.ToLower();
                row++;
                if (itemLower == "*")
                {
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(30, GridUnitType.Absolute) });
                    grid.Add(GetLabel(inputs.Index.ToString()), 0, row);
                    grid.Add(GetBorder(), 0, row);
                }
                else if (itemLower == "data")
                {
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    var label = new Label
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        HorizontalTextAlignment = TextAlignment.Center,
                        FormattedText = new()
                    };
                    label.FormattedText.Spans.Add(new Span() { Text = inputs.Day.SelectedDate.ToString("dd.MM.yyyy") });
                    label.FormattedText.Spans.Add(new Span() { Text = Environment.NewLine });
                    label.FormattedText.Spans.Add(new Span() { Text = TranslateDayOfWeekStatic.TranslateSelectedDay(inputs.Day.SelectedDate.DayOfWeek) });
                    grid.Add(label, 0, row);
                    grid.Add(GetBorder(), 0, row);
                }
                else if (itemLower == "kierowca")
                {
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    var user = new Shared.CustomControls.NameOrIdUser
                    {
                        UserId = inputs.Day.UserCreatedId,
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        HorizontalTextAlignment = TextAlignment.Center
                    };
                    grid.Add(user, 0, row);
                    grid.Add(GetBorder(), 0, row);
                }
                else
                {
                    RangeDayVM.GetKeyAndNameFromHeader(itemLower, out string key, out string name);
                    decimal? selectedProduct = RangeDayVM.GetValueFromName(inputs.Day, TranslateHeader(key), name);
                    if (selectedProduct is null)
                    {
                        row--;
                        continue;
                    }
                    grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    grid.Add(GetLabel(selectedProduct.ToString()), 0, row);
                    grid.Add(GetBorder(), 0, row);
                }
            }
            if (row == -1)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(inputs.SelectedValue))
            {
                row++;
                grid.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.Add(GetLabel(inputs.SelectedValue.ToString()), 0, row);
            }

            grid.AddWithSpan(
                new Border()
                {
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.End, false)
                }
                , 0, 0, row + 1, 1);
        }

        public static string TranslateHeader(string pl)
        {
            return pl.ToLower() switch
            {
                "ilość" => nameof(Product.Number),
                "edycja" => nameof(Product.NumberEdit),
                "zwrot" => nameof(Product.NumberReturn),
                "sprzedane" => "Sell",
                "po korekcie" => nameof(Product.PriceTotalAfterCorrectDecimal),
                "korekta" => nameof(Product.PriceTotalCorrectDecimal),
                "utarg" => nameof(Product.PriceTotalDecimal),

                "zapłacono" => nameof(Day.TotalPriceMoneyDecimal),
                "utarg produkty" => nameof(Day.TotalPriceProductsDecimal),
                "utarg ciasto" => nameof(Day.TotalPriceCakeDecimal),
                "utarg suma" => nameof(Day.TotalPriceDecimal),
                "różnica" => nameof(Day.TotalPriceDifferenceDecimal),
                "*" => "*",
                "data" => nameof(Day.SelectedDate),
                "kierowca" => nameof(Day.UserCreatedId),
                _ => "",
            };
        }
        private static Label GetLabel(string header)
        {
            var label = new Label
            {
                Text = header,
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
