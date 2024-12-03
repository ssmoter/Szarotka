using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.ExistingFiles
{
    public partial class ExistingFilesM : ObservableObject
    {
        [ObservableProperty]
        string path;

        [ObservableProperty]
        string name;

    }
}
