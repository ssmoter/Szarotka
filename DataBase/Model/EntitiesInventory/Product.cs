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
                CalculatePrice();
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
                CalculatePrice();
            }
        }
    }
    private int priceTotalCorrect;
    public int PriceTotalCorrect
    {
        get => priceTotalCorrect;
        set
        {
            if (SetProperty(ref priceTotalCorrect, value))
            {
                OnPropertyChanged(nameof(PriceTotalCorrect));
                OnPropertyChanged(nameof(PriceTotalCorrectDecimal));
                CalculatePrice();
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
                CalculatePrice();
            }
        }
    }
    private int priceTotalAfterCorrect;
    public int PriceTotalAfterCorrect
    {
        get => priceTotalAfterCorrect;
        set
        {
            if (SetProperty(ref priceTotalAfterCorrect, value))
            {
                OnPropertyChanged(nameof(PriceTotalAfterCorrect));
                OnPropertyChanged(nameof(PriceTotalAfterCorrectDecimal));
                CalculatePrice();
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
                CalculatePrice();
            }
        }
    }



    private int number;
    public int Number
    {
        get => number;
        set
        {
            if (SetProperty(ref number, value))
            {
                OnPropertyChanged(nameof(Number));
                CalculatePrice();
            }
        }
    }
    private int numberEdit;
    public int NumberEdit
    {
        get => numberEdit;
        set
        {
            if (SetProperty(ref numberEdit, value))
            {
                OnPropertyChanged(nameof(NumberEdit));
                CalculatePrice();
            }
        }
    }

    private int numberReturn;
    public int NumberReturn
    {
        get => numberReturn;
        set
        {
            if (SetProperty(ref numberReturn, value))
            {
                OnPropertyChanged(nameof(NumberReturn));
                CalculatePrice();
            }
        }
    }

    [Ignore]
    public bool CanUpadte { get; set; }

    private bool isExpanded;
    [Ignore]
    public bool IsExpanded
    {
        get => isExpanded;
        set
        {
            if (SetProperty(ref isExpanded, value))
            {
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
    }

    public Product()
    {
        Name ??= new();
        Price ??= new();
    }
    public Product(Product product)
    {
        this.Id = product.Id;
        this.Created = product.Created;
        this.Updated = product.Updated;

        this.DayId = product.DayId;
        this.ProductNameId = product.ProductNameId;
        this.ProductPriceId = product.ProductPriceId;
        this.Description = product.Description;

        this.Name = product.Name;
        this.Price = product.Price;

        this.PriceTotalDecimal = product.PriceTotalDecimal;
        this.PriceTotalCorrectDecimal = product.PriceTotalCorrectDecimal;
        this.PriceTotalAfterCorrectDecimal = product.PriceTotalAfterCorrectDecimal;

        this.Number = product.Number;
        this.NumberEdit = product.NumberEdit;
        this.NumberReturn = product.NumberReturn;

        this.CanUpadte = product.CanUpadte;
    }
    public void CalculatePrice()
    {
        if (CanUpadte)
        {
            PriceTotalDecimal = (number + numberEdit - numberReturn) * Price.PriceDecimal;
            PriceTotalAfterCorrectDecimal = PriceTotalDecimal + PriceTotalCorrectDecimal;
            ProductUpdatePriceService.OnUpdate();
        }
    }
}