using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class ProductPrice : BaseEntities<Guid>
{
    [ObservableProperty]
    private Guid productNameId;
    private int price;
    public int Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value))
            {
                OnPropertyChanged(nameof(Price));
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
            if (SetProperty(ref price, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDecimal));
            }
        }
    }

}
