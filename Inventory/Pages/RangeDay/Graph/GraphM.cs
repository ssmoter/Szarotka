using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.Graph
{
    public partial class GraphM : ObservableObject
    {
        [ObservableProperty]
        string name;
        [ObservableProperty]
        Color color;
    }


}
