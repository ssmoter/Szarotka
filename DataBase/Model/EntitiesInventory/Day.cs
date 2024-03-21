using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class Day : BaseEntities<Guid>
{
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private Guid driverGuid;
    private string selectedDateString;
    public string SelectedDateString
    {
        get => selectedDateString;
        set
        {
            if (SetProperty(ref selectedDateString, value))
            {
                OnPropertyChanged(nameof(SelectedDateString));
            }
        }
    }

    [Ignore]
    public DateTime SelectedDate
    {
        get => new(selectedDateTicks);
        set
        {
            if (SetProperty(ref selectedDateString, value.ToString("dd.MM.yyyy")))
                OnPropertyChanged(nameof(SelectedDateString));
            if (SetProperty(ref selectedDateTicks, value.Ticks))
                OnPropertyChanged(nameof(SelectedDateTicks));
        }
    }

    private long selectedDateTicks;
    public long SelectedDateTicks
    {
        get => selectedDateTicks;
        set
        {
            if (SetProperty(ref selectedDateTicks, value))
            {
                OnPropertyChanged(nameof(SelectedDateTicks));
            }
        }
    }
    private int totalPriceProducts;
    public int TotalPriceProducts
    {
        get => totalPriceProducts;
        set
        {
            if (SetProperty(ref totalPriceProducts, value))
            {
                OnPropertyChanged(nameof(TotalPriceProducts));
                OnPropertyChanged(nameof(TotalPriceProductsDecimal));
            }
        }
    }
    [Ignore]
    public decimal TotalPriceProductsDecimal
    {
        get
        {
            return (decimal)totalPriceProducts / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceProducts, (int)(value * 100)))
                OnPropertyChanged(nameof(TotalPriceProductsDecimal));
        }
    }
    private int totalPriceCake;
    public int TotalPriceCake
    {
        get => totalPriceCake;
        set
        {
            if (SetProperty(ref totalPriceCake, value))
            {
                OnPropertyChanged(nameof(TotalPriceCake));
                OnPropertyChanged(nameof(TotalPriceCakeDecimal));
            }
        }
    }
    [Ignore]
    public decimal TotalPriceCakeDecimal
    {
        get
        {
            return (decimal)totalPriceCake / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceCake, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPriceCake));
                OnPropertyChanged(nameof(TotalPriceCakeDecimal));
            }
        }
    }
    private int totalPrice;
    public int TotalPrice
    {
        get => totalPrice;
        set
        {
            if (SetProperty(ref totalPrice, value))
            {
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(TotalPriceDecimal));
            }
        }
    }

    [Ignore]
    public decimal TotalPriceDecimal
    {
        get
        {
            return (decimal)totalPrice / 100m;
        }
        set
        {
            if (SetProperty(ref totalPrice, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(TotalPriceDecimal));
            }
        }
    }
    private int totalPriceCorrect;
    public int TotalPriceCorrect
    {
        get => totalPriceCorrect;
        set
        {
            if (SetProperty(ref totalPriceCorrect, value))
            {
                OnPropertyChanged(nameof(TotalPriceCorrect));
                OnPropertyChanged(nameof(TotalPriceCorrectDecimal));
            }
        }
    }

    [Ignore]
    public decimal TotalPriceCorrectDecimal
    {
        get
        {
            return (decimal)totalPriceCorrect / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceCorrect, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPriceCorrect));
                OnPropertyChanged(nameof(TotalPriceCorrectDecimal));
            }
        }
    }
    private int totalPriceAfterCorrect;
    public int TotalPriceAfterCorrect
    {
        get => totalPriceAfterCorrect;
        set
        {
            if (SetProperty(ref totalPriceAfterCorrect, value))
            {
                OnPropertyChanged(nameof(TotalPriceAfterCorrect));
                OnPropertyChanged(nameof(TotalPriceAfterCorrectDecimal));
            }
        }
    }

    [Ignore]
    public decimal TotalPriceAfterCorrectDecimal
    {
        get
        {
            return (decimal)totalPriceAfterCorrect / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceAfterCorrect, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPriceAfterCorrect));
                OnPropertyChanged(nameof(TotalPriceAfterCorrectDecimal));
            }
        }
    }
    private int totalPriceMoney;
    public int TotalPriceMoney
    {
        get => totalPriceMoney;
        set
        {
            if (SetProperty(ref totalPriceMoney, value))
            {
                OnPropertyChanged(nameof(TotalPriceMoney));
                OnPropertyChanged(nameof(TotalPriceMoneyDecimal));
            }
        }
    }

    [Ignore]
    public decimal TotalPriceMoneyDecimal
    {
        get
        {
            return (decimal)totalPriceMoney / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceMoney, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPriceMoney));
                OnPropertyChanged(nameof(TotalPriceMoneyDecimal));
            }
        }
    }
    private int totalPriceDifference;
    public int TotalPriceDifference
    {
        get => totalPriceDifference;
        set
        {
            if (SetProperty(ref totalPriceDifference, value))
            {
                OnPropertyChanged(nameof(TotalPriceDifference));
                OnPropertyChanged(nameof(TotalPriceDifferenceDecimal));
            }
        }
    }

    [Ignore]
    public decimal TotalPriceDifferenceDecimal
    {
        get
        {
            return (decimal)totalPriceDifference / 100m;
        }
        set
        {
            if (SetProperty(ref totalPriceDifference, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(TotalPriceDifference));
                OnPropertyChanged(nameof(TotalPriceDifferenceDecimal));
            }
        }
    }

    private List<Product> products;
    [Ignore]
    public List<Product> Products
    {
        get => products;
        set
        {
            if (SetProperty(ref products, value))
            {
                OnPropertyChanged(nameof(Products));
            }
        }
    }
    private List<Cake> cakes;
    [Ignore]
    public List<Cake> Cakes
    {
        get => cakes;
        set
        {
            if (SetProperty(ref cakes, value))
            {
                OnPropertyChanged(nameof(Cakes));
            }
        }
    }

    public Day()
    {
        Products = [];
        Cakes = [];
    }
}
