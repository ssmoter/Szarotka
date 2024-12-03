using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using System.Collections.ObjectModel;

namespace DataBase.Model.EntitiesInventory;

public partial class Day : BaseEntities<Guid>, IDisposable
{
    [ObservableProperty]
    private string description = "";
    [ObservableProperty]
    private Guid driverGuid;
    private string selectedDateString = "";
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
                UpdateTotalPrice();
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
            {
                OnPropertyChanged(nameof(TotalPriceProductsDecimal));
                UpdateTotalPrice();
            }

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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
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
                UpdateTotalPrice();
            }
        }
    }

    private ObservableCollection<Product> products = [];
    [Ignore]
    public ObservableCollection<Product> Products
    {
        get => products;
        set
        {
            if (SetProperty(ref products, value))
            {
                OnPropertyChanged(nameof(Products));
                UpdateTotalPrice();
            }
        }
    }
    private ObservableCollection<Cake> cakes = [];
    [Ignore]
    public ObservableCollection<Cake> Cakes
    {
        get => cakes;
        set
        {
            if (SetProperty(ref cakes, value))
            {
                OnPropertyChanged(nameof(Cakes));
                UpdateTotalPrice();
            }
        }
    }
    [Ignore]
    public bool CanUpadte { get; set; }

    public void UpdateTotalPrice()
    {
        if (CanUpadte)
        {
            if (Products is not null)
                TotalPriceProductsDecimal = Products.Sum(x => x.PriceTotalAfterCorrectDecimal);
            if (Cakes is not null)
                TotalPriceCakeDecimal = Cakes.Where(x => x.IsSell).Sum(x => x.PriceDecimal);

            TotalPriceDecimal = TotalPriceProductsDecimal + TotalPriceCakeDecimal;
            TotalPriceAfterCorrectDecimal = TotalPriceDecimal + TotalPriceCorrectDecimal;

            TotalPriceDifferenceDecimal = TotalPriceMoneyDecimal - TotalPriceAfterCorrectDecimal;
        }
    }



    public Day()
    {
        Products ??= [];
        Cakes ??= [];
        ProductUpdatePriceService.UpdatePrice += UpdateTotalPrice;
    }
    public Day(Day day)
    {
        this.Products = day.Products;
        this.Cakes = day.Cakes;

        this.Id = day.Id;
        this.driverGuid = day.DriverGuid;
        this.Created = day.Created;
        this.Updated = day.Updated;
        this.description = day.Description;
        this.SelectedDateTicks = day.SelectedDateTicks;
        this.TotalPriceProductsDecimal = day.TotalPriceProductsDecimal;
        this.TotalPriceDecimal = day.TotalPriceDecimal;
        this.TotalPriceCorrectDecimal = day.TotalPriceCorrectDecimal;
        this.TotalPriceMoneyDecimal = day.TotalPriceMoneyDecimal;
        this.TotalPriceDifferenceDecimal = day.TotalPriceDifferenceDecimal;

        ProductUpdatePriceService.UpdatePrice += UpdateTotalPrice;
    }
    public void Dispose()
    {
        Products.Clear();
        Cakes.Clear();
        ProductUpdatePriceService.UpdatePrice -= UpdateTotalPrice;
        GC.SuppressFinalize(this);
    }

}
public static class ProductUpdatePriceService
{
    public static event Action? UpdatePrice;
    /// <summary>
    /// Aktualizowanie utargu
    /// </summary>
    public static void OnUpdate()
    {
        UpdatePrice?.Invoke();
    }
}