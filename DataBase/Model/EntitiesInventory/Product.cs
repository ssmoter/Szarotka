using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class Product : BaseEntities<Guid>
{
    [ObservableProperty]
    private Guid dayId;
    [ObservableProperty]
    private Guid productNameId;
    [ObservableProperty]
    private Guid productPriceId;
    private ProductName name;
    [Ignore]
    public ProductName Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value))
            {
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    [ObservableProperty]
    private string description;

    private ProductPrice price;
    [Ignore]
    public ProductPrice Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value))
            {
                OnPropertyChanged(nameof(Price));
            }
        }
    }
    private int priceTotal;
    public int PriceTotal
    {
        get => priceTotal;
        set
        {
            if (SetProperty(ref priceTotal, value))
            {
                OnPropertyChanged(nameof(PriceTotal));
                OnPropertyChanged(nameof(PriceTotalDecimal));
            }
        }
    }


    [Ignore]
    public decimal PriceTotalDecimal
    {
        get
        {
            return (decimal)priceTotal / 100m;
        }
        set
        {
            if (SetProperty(ref priceTotal, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(PriceTotal));
                OnPropertyChanged(nameof(PriceTotalDecimal));
            }
        }
    }
    private int priceTotalCorrect;
    public int PriceTotalCorrect
    {
        get => priceTotalAfterCorrect;
        set
        {
            if (SetProperty(ref priceTotalCorrect, value))
            {
                OnPropertyChanged(nameof(PriceTotalCorrect));
                OnPropertyChanged(nameof(PriceTotalCorrectDecimal));
            }
        }
    }

    [Ignore]
    public decimal PriceTotalCorrectDecimal
    {
        get
        {
            return (decimal)priceTotalCorrect / 100m;
        }
        set
        {
            if (SetProperty(ref priceTotalCorrect, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(PriceTotalCorrect));
                OnPropertyChanged(nameof(PriceTotalCorrectDecimal));
            }
        }
    }
    private int priceTotalAfterCorrect;
    public int PriceTotalAfterCorrect
    {
        get => priceTotalAfterCorrect;
        set
        {
            if (SetProperty(ref priceTotalAfterCorrect, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(PriceTotalAfterCorrect));
                OnPropertyChanged(nameof(PriceTotalAfterCorrectDecimal));
            }
        }
    }
    [Ignore]
    public decimal PriceTotalAfterCorrectDecimal
    {
        get
        {
            return (decimal)priceTotalAfterCorrect / 100m;
        }
        set
        {
            if (SetProperty(ref priceTotalAfterCorrect, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(PriceTotalAfterCorrect));
                OnPropertyChanged(nameof(PriceTotalAfterCorrectDecimal));
            }
        }
    }
    [ObservableProperty]
    private int number;
    [ObservableProperty]
    private int numberEdit;
    [ObservableProperty]
    private int numberReturn;
}