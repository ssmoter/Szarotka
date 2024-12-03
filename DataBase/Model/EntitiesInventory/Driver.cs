using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesInventory;

public partial class Driver : BaseEntities<Guid>
{
    [ObservableProperty]
    private string name="";
    [ObservableProperty]
    private string description="";
}
public partial class SelectedDriver : BaseEntities<int>
{
    [ObservableProperty]
    private Guid selectedGuid;
}

