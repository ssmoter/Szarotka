using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct
{
    public partial class ListProductM : ObservableObject
    {
        [ObservableProperty]
        Inventory.Model.MVVM.ProductNameM name;

        [ObservableProperty]
        decimal actualPrice;

        [ObservableProperty]
        string actualCreated;

        [ObservableProperty]
        ObservableCollection<Inventory.Model.MVVM.ProductPriceM> prices;

        public ListProductM()
        {
            Name = new Model.MVVM.ProductNameM();
            Prices = new ObservableCollection<Model.MVVM.ProductPriceM>();
        }

        public void SetActualPrice()
        {
            if (Prices is not null)
            {
                if (Prices.Count > 0)
                {
                    ActualPrice = Prices.FirstOrDefault().Price;
                    ActualCreated = Prices.FirstOrDefault().Created.ToShortDateString();
                }
            }
        }
    }
}
