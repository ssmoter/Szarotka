using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.ExistingFiles
{
    public partial class ExistingFilesM : ObservableObject
    {
        [ObservableProperty]
        string path;

        [ObservableProperty]
        string name;

    }
}
