using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class Product : BaseEntities<Guid>
{
    private Guid dayId;
    public Guid DayId
    {
        get => dayId;
        set
        {
            if (SetProperty(ref dayId, value, nameof(DayId)))
            {
                //OnPropertyChanged(nameof(DayId));
            }
        }
    }
    private Guid productNameId;
    public Guid ProductNameId
    {
        get => productNameId;
        set
        {
            if (SetProperty(ref productNameId, value, nameof(ProductNameId)))
            {
                //OnPropertyChanged(nameof(ProductNameId));
            }
        }
    }
    private Guid productPriceId;
    public Guid ProductPriceId
    {
        get => productPriceId;
        set
        {
            if (SetProperty(ref productPriceId, value, nameof(ProductPriceId)))
            {
                //OnPropertyChanged(nameof(ProductPriceId));
            }
        }
    }
    private ProductName name = new();
    [Ignore]
    public ProductName Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name)))
            {
                //OnPropertyChanged(nameof(Name));
            }
        }
    }
    private string description = "";
    public string Description
    {
        get => description;
        set
        {
            if (SetProperty(ref description, value, nameof(Description))) { }
        }
    }

    private ProductPrice price = new();
    [Ignore]
    public ProductPrice Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value, nameof(Price)))
            {
                //OnPropertyChanged(nameof(Price));
            }
        }
    }
    private int priceTotal;
    public int PriceTotal
    {
        get => priceTotal;
        set
        {
            if (SetProperty(ref priceTotal, value, nameof(PriceTotal)))
            {
                //OnPropertyChanged(nameof(PriceTotal));
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
            if (SetProperty(ref priceTotal, (int)(value * 100), nameof(PriceTotalDecimal)))
            {
                OnPropertyChanged(nameof(PriceTotal));
                //OnPropertyChanged(nameof(PriceTotalDecimal));

            }
        }
    }
    private int priceTotalCorrect;
    public int PriceTotalCorrect
    {
        get => priceTotalCorrect;
        set
        {
            if (SetProperty(ref priceTotalCorrect, value, nameof(PriceTotalCorrect)))
            {
                //OnPropertyChanged(nameof(PriceTotalCorrect));
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
            if (SetProperty(ref priceTotalCorrect, (int)(value * 100), nameof(PriceTotalCorrectDecimal)))
            {
                OnPropertyChanged(nameof(PriceTotalCorrect));
                //OnPropertyChanged(nameof(PriceTotalCorrectDecimal));

            }
        }
    }
    private int priceTotalAfterCorrect;
    public int PriceTotalAfterCorrect
    {
        get => priceTotalAfterCorrect;
        set
        {
            if (SetProperty(ref priceTotalAfterCorrect, value, nameof(PriceTotalAfterCorrect)))
            {
                //OnPropertyChanged(nameof(PriceTotalAfterCorrect));
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
            if (SetProperty(ref priceTotalAfterCorrect, (int)(value * 100), nameof(PriceTotalAfterCorrectDecimal)))
            {
                OnPropertyChanged(nameof(PriceTotalAfterCorrect));
                //OnPropertyChanged(nameof(PriceTotalAfterCorrectDecimal));

            }
        }
    }



    private int number;
    public int Number
    {
        get => number;
        set
        {
            if (SetProperty(ref number, value, nameof(Number)))
            {
                //OnPropertyChanged(nameof(Number));

            }
        }
    }
    private int numberEdit;
    public int NumberEdit
    {
        get => numberEdit;
        set
        {
            if (SetProperty(ref numberEdit, value, nameof(NumberEdit)))
            {
                //OnPropertyChanged(nameof(NumberEdit));

            }
        }
    }

    private int numberReturn;
    public int NumberReturn
    {
        get => numberReturn;
        set
        {
            if (SetProperty(ref numberReturn, value, nameof(NumberReturn)))
            {
                //OnPropertyChanged(nameof(NumberReturn));

            }
        }
    }

    private bool isExpanded;
    [Ignore]
    public bool IsExpanded
    {
        get => isExpanded;
        set
        {
            if (SetProperty(ref isExpanded, value, nameof(IsExpanded)))
            {
                //OnPropertyChanged(nameof(IsExpanded));
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

        this.IsDelete = product.IsDelete;

    }
    public void CalculatePrice()
    {
        PriceTotalDecimal = (number + numberEdit - numberReturn) * Price.PriceDecimal;
        PriceTotalAfterCorrectDecimal = PriceTotalDecimal + PriceTotalCorrectDecimal;
    }

}