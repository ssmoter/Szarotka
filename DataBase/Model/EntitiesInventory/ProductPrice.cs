using SQLite;

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

}
