using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct
{
    public partial class ListProductM : ObservableObject
    {
        [ObservableProperty]
        ProductName name;

        [ObservableProperty]
        decimal actualPrice;

        [ObservableProperty]
        string actualCreated;

        [ObservableProperty]
        ObservableCollection<ProductPrice> prices;

        public ListProductM()
        {
            Name = new();
            Prices = [];
        }

        public void SetActualPrice()
        {
            if (Prices is not null)
            {
                if (Prices.Count > 0)
                {
                    ActualPrice = Prices.FirstOrDefault().PriceDecimal;
                    ActualCreated = Prices.FirstOrDefault().Created.ToShortDateString();
                }
            }
        }
    }
}
