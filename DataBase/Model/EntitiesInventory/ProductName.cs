using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesInventory;

public partial class ProductName : BaseEntities<Guid>
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

}
