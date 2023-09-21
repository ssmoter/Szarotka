using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class DriverM : ObservableObject
    {
        [ObservableProperty]
        int id;
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [ObservableProperty]
        Guid guid;
    }
}
