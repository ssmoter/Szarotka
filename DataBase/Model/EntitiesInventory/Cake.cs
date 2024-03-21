using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class Cake : BaseEntities<Guid>
{
    [ObservableProperty]
    private Guid dayId;
    [ObservableProperty]
    private bool isSell;


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
            return (decimal)price / 100m;
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

