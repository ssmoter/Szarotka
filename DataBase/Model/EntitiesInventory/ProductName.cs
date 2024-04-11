using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesInventory;

public partial class ProductName : BaseEntities<Guid>, IEquatable<ProductName>
{
    [ObservableProperty]
    private int arrangement;
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private string img;
    [ObservableProperty]
    private bool isVisible = true;

    public override bool Equals(object obj) => Equals(obj as ProductName);
    public override int GetHashCode() => (Id, Name).GetHashCode();
    public bool Equals(ProductName other)
    {
        if (other is null)
            return false;

        return Id == other.Id && Name == other.Name;
    }
}
