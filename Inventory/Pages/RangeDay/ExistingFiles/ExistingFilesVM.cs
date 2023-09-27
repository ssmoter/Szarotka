using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace Inventory.Pages.RangeDay.ExistingFiles
{
    [QueryProperty(nameof(ExistingFilesM), nameof(ExistingFiles.ExistingFilesM))]
    [QueryProperty(nameof(GetTyp), nameof(GetTyp))]
    public partial class ExistingFilesVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ExistingFilesM> existingFilesM;

        [ObservableProperty]
        string getTyp;

        public ExistingFilesVM()
        {
            ExistingFilesM = new ObservableCollection<ExistingFilesM>();
        }


        [RelayCommand]
        async Task SelectedImport(ExistingFilesM filesM)
        {
            if (filesM is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(RangeDayV)}?FilesPath={filesM.Path}");

        }
        [RelayCommand]
        async Task SelectedExport(ExistingFilesM filesM)
        {
            if (filesM is null)
            {
                return;
            }
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = filesM.Name,
                File = new ShareFile(filesM.Path)
            });
        }

    }
}
