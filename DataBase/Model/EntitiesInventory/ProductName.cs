using DataBase.Model.SourceGenerator;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesInventory;

public partial class ProductName : BaseEntities<Guid>, IEquatable<ProductName>
{
    private int arrangement;
    public int Arrangement
    {
        get => arrangement;
        set
        {
            if (SetProperty(ref arrangement, value, nameof(Arrangement)))
            {
                //OnPropertyChanged(nameof(Arrangement));
            }
        }
    }
    private string name = "";
    public string Name
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
            if (SetProperty(ref description, value, nameof(Description)))
            {
                //OnPropertyChanged(nameof(Description));
            }
        }
    }
    private string img = "";
    public string Img
    {
        get => img;
        set
        {
            if (SetProperty(ref img, value, nameof(Img)))
            {
                //OnPropertyChanged(nameof(Img));
            }
        }
    }
    private bool isVisible = true;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (SetProperty(ref isVisible, value, nameof(IsVisible)))
            {
                //OnPropertyChanged(nameof(IsVisible));
            }
        }
    }
    public override bool Equals(object? obj) => Equals(obj as ProductName);
    public override int GetHashCode() => (Id, Name).GetHashCode();
    public bool Equals(ProductName? other)
    {
        if (other is null)
            return false;

        return Id == other.Id && Name == other.Name;
    }

    public ProductName()
    {
        
    }
    public ProductName(ProductName copy):base(copy)
    {
        this.Arrangement = copy.Arrangement;
        this.Name = copy.Name;
        this.Img = copy.Img;
        this.IsVisible = copy.IsVisible;
        this.Description = copy.Description;
    }
}


public partial class EmptyProduct
{
    public ProductName Name { get; set; } = new();
    public IList<ProductPrice> Prices { get; set; } = [];
}
public partial class EmptyProducts
{
    public IList<EmptyProduct> Products { get; set; } = [];

    public EmptyProducts()
    {
        Products ??= [];
    }
    public EmptyProducts(IList<(ProductName Name, IList<ProductPrice> Prices)> emptyProducts)
    {
        int count = emptyProducts.Count;
        Products = new EmptyProduct[count];

        for (int i = 0; i < count; i++)
        {
            Products[i] = new()
            {
                Name = emptyProducts[i].Name,
                Prices = emptyProducts[i].Prices
            };
        }

    }
    public EmptyProducts(IList<ProductName> name)
    {
        var length = name.Count;
        Products = new EmptyProduct[length];
        for (int i = 0; i < length; i++)
        {
            Products[i] = new()
            {
                Name = name[i]
            };
        }
    }
    public EmptyProducts(IEnumerable<EmptyProduct> products)
    {
        Products = [.. products];
    }
}