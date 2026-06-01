using SQLite;

using System.Collections.ObjectModel;

namespace DataBase.Model.EntitiesInventory;

public partial class ProductPrice : BaseEntities<Guid>
{
    private Guid productNameId;
    public Guid ProductNameId
    {
        get => productNameId;
        set
        {
            if (SetProperty(ref productNameId, value, nameof(ProductNameId)))
            {
                //OnPropertyChanged(nameof(productNameId));
            }
        }
    }
    private int price;
    public int Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value, nameof(Price)))
            {
                //OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDecimal));
            }
        }
    }
    [Ignore]
    public decimal PriceDecimal
    {
        get
        {
            return (decimal)Price / 100m;
        }
        set
        {
            if (SetProperty(ref price, (int)(value * 100), nameof(PriceDecimal)))
            {
                OnPropertyChanged(nameof(Price));
                //OnPropertyChanged(nameof(PriceDecimal));
            }
        }
    }
    public ProductPrice()
    {

    }
    public ProductPrice(ProductPrice copy) : base(copy)
    {
        Price = copy.Price;
        PriceDecimal = copy.PriceDecimal;
        ProductNameId = copy.ProductNameId;
    }
}

public partial class ProductPrices : BaseEntities<int>
{
    private ObservableCollection<ProductPrice> prices = [];
    [Ignore]
    public ObservableCollection<ProductPrice> Prices
    {
        get => prices;
        set
        {
            if (SetProperty(ref prices, value, nameof(Prices)))
            {
            }
        }
    }
}
