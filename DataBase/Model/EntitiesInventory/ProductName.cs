using DataBase.Model.JsonContext;

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
}
