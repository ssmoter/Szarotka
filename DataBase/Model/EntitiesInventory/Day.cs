using SQLite;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesInventory;


public partial class Day : BaseEntities<Guid>, IDisposable
{
    private string description = "";
    public string Description
    {
        get => description;
        set
        {
            if (SetProperty(ref description, value, nameof(Description)))
            {
                //OnPropertyChanged(nameof(Description));
            }
        }
    }

    private Guid driverGuid;
    public Guid DriverGuid
    {
        get => driverGuid;
        set
        {
            if (SetProperty(ref driverGuid, value, nameof(DriverGuid)))
            {
                //OnPropertyChanged(nameof(DriverGuid));
            }
        }
    }
    private string selectedDateString = "";
    public string SelectedDateString
    {
        get => selectedDateString;
        set
        {
            if (SetProperty(ref selectedDateString, value, nameof(SelectedDateString)))
            {
                //OnPropertyChanged(nameof(SelectedDateString));
            }
        }
    }

    [Ignore]
    [JsonConverter(typeof(JsonContext.CustomDateTimeConverter))]
    public DateTime SelectedDate
    {
        get => new(selectedDateTicks);
        set
        {
            if (SetProperty(ref selectedDateString, value.ToString("dd.MM.yyyy"), nameof(SelectedDate)))
            {
                OnPropertyChanged(nameof(SelectedDateString));
            }
            if (SetProperty(ref selectedDateTicks, value.Ticks))
            {
                OnPropertyChanged(nameof(SelectedDateTicks));
            }
        }
    }

    private long selectedDateTicks;
    public long SelectedDateTicks
    {
        get => selectedDateTicks;
        set
        {
            if (SetProperty(ref selectedDateTicks, value, nameof(SelectedDateTicks)))
            {
                //OnPropertyChanged(nameof(SelectedDateTicks));
            }
        }
    }
    private int totalPriceProducts;
    public int TotalPriceProducts
    {
        get => totalPriceProducts;
        set
        {
            if (SetProperty(ref totalPriceProducts, value, nameof(TotalPriceProducts)))
            {
                //OnPropertyChanged(nameof(TotalPriceProducts));
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
            if (SetProperty(ref totalPriceProducts, (int)(value * 100), nameof(TotalPriceProductsDecimal)))
            {
                //OnPropertyChanged(nameof(TotalPriceProductsDecimal));
                OnPropertyChanged(nameof(TotalPriceProducts));

            }

        }
    }
    private int totalPriceCake;
    public int TotalPriceCake
    {
        get => totalPriceCake;
        set
        {
            if (SetProperty(ref totalPriceCake, value, nameof(TotalPriceCake)))
            {
                //OnPropertyChanged(nameof(TotalPriceCake));
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
            if (SetProperty(ref totalPriceCake, (int)(value * 100), nameof(TotalPriceCakeDecimal)))
            {
                OnPropertyChanged(nameof(TotalPriceCake));
                //OnPropertyChanged(nameof(TotalPriceCakeDecimal));

            }
        }
    }
    private int totalPrice;
    public int TotalPrice
    {
        get => totalPrice;
        set
        {
            if (SetProperty(ref totalPrice, value, nameof(TotalPrice)))
            {
                //OnPropertyChanged(nameof(TotalPrice));
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
            if (SetProperty(ref totalPrice, (int)(value * 100), nameof(TotalPriceDecimal)))
            {
                OnPropertyChanged(nameof(TotalPrice));
                //OnPropertyChanged(nameof(TotalPriceDecimal));

            }
        }
    }
    private int totalPriceCorrect;
    public int TotalPriceCorrect
    {
        get => totalPriceCorrect;
        set
        {
            if (SetProperty(ref totalPriceCorrect, value, nameof(TotalPriceCorrect)))
            {
                //OnPropertyChanged(nameof(TotalPriceCorrect));
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
            if (SetProperty(ref totalPriceCorrect, (int)(value * 100), nameof(TotalPriceCorrectDecimal)))
            {
                OnPropertyChanged(nameof(TotalPriceCorrect));
                //OnPropertyChanged(nameof(TotalPriceCorrectDecimal));

            }
        }
    }
    private int totalPriceAfterCorrect;
    public int TotalPriceAfterCorrect
    {
        get => totalPriceAfterCorrect;
        set
        {
            if (SetProperty(ref totalPriceAfterCorrect, value, nameof(TotalPriceAfterCorrect)))
            {
                //OnPropertyChanged(nameof(TotalPriceAfterCorrect));
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
            if (SetProperty(ref totalPriceAfterCorrect, (int)(value * 100), nameof(TotalPriceAfterCorrectDecimal)))
            {
                OnPropertyChanged(nameof(TotalPriceAfterCorrect));
                //OnPropertyChanged(nameof(TotalPriceAfterCorrectDecimal));

            }
        }
    }
    private int totalPriceMoney;
    public int TotalPriceMoney
    {
        get => totalPriceMoney;
        set
        {
            if (SetProperty(ref totalPriceMoney, value, nameof(TotalPriceMoney)))
            {
                //OnPropertyChanged(nameof(TotalPriceMoney));
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
            if (SetProperty(ref totalPriceMoney, (int)(value * 100), nameof(TotalPriceMoneyDecimal)))
            {
                OnPropertyChanged(nameof(TotalPriceMoney));
                //OnPropertyChanged(nameof(TotalPriceMoneyDecimal));

            }
        }
    }
    private int totalPriceDifference;
    public int TotalPriceDifference
    {
        get => totalPriceDifference;
        set
        {
            if (SetProperty(ref totalPriceDifference, value, nameof(TotalPriceDifference)))
            {
                //OnPropertyChanged(nameof(TotalPriceDifference));
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
            if (SetProperty(ref totalPriceDifference, (int)(value * 100), nameof(TotalPriceDifferenceDecimal)))
            {
                OnPropertyChanged(nameof(TotalPriceDifference));
                //OnPropertyChanged(nameof(TotalPriceDifferenceDecimal));

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
            if (SetProperty(ref products, value, nameof(Products)))
            {
                //OnPropertyChanged(nameof(Products));

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
            if (SetProperty(ref cakes, value, nameof(Cakes)))
            {
                //OnPropertyChanged(nameof(Cakes));

            }
        }
    }

    public void UpdateTotalPrice()
    {
        if (Products is not null)
            TotalPriceProductsDecimal = Products.Where(x => !x.IsDelete).Sum(z => z.PriceTotalAfterCorrectDecimal);
        if (Cakes is not null)
            TotalPriceCakeDecimal = Cakes.Where(x => x.IsSell && !x.IsDelete).Sum(x => x.PriceDecimal);

        TotalPriceDecimal = TotalPriceProductsDecimal + TotalPriceCakeDecimal;
        TotalPriceAfterCorrectDecimal = TotalPriceDecimal + TotalPriceCorrectDecimal;
        TotalPriceDifferenceDecimal = TotalPriceMoneyDecimal - TotalPriceAfterCorrectDecimal;
    }



    public Day()
    {
        Products ??= [];
        Cakes ??= [];
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
        this.IsDelete = day.IsDelete;
    }
    public void Dispose()
    {
        Products.Clear();
        Cakes.Clear();
        GC.SuppressFinalize(this);
    }

}