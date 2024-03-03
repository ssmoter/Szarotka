using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Pages.ExistingFiles
{
    public partial class ExistingFilesM : ObservableObject
    {
        [ObservableProperty]
        string path;

        [ObservableProperty]
        string name;

    }
}
