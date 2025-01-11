using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct;

public partial class ListProductM : ObservableObject
{
    private ProductName name;
    public ProductName Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name))) { }
        }
    }

    private decimal actualPrice;
    public decimal ActualPrice
    {
        get => actualPrice;
        set
        {
            if (SetProperty(ref actualPrice, value, nameof(ActualPrice))) { }
        }
    }

    private string actualCreated;
    public string ActualCreated
    {
        get => actualCreated;
        set
        {
            if (SetProperty(ref actualCreated, value, nameof(ActualCreated))) { }
        }
    }
    private ObservableCollection<ProductPrice> prices;
    public ObservableCollection<ProductPrice> Prices
    {
        get => prices;
        set
        {
            if (SetProperty(ref prices, value, nameof(Prices))) { }
        }
    }
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


