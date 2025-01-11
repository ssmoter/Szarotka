using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesInventory;


public partial class Driver : BaseEntities<Guid>
{
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
}
public partial class SelectedDriver : BaseEntities<int>
{
    [ObservableProperty]
    private Guid selectedGuid;
}

