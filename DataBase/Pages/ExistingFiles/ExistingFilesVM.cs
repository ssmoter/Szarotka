using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace DataBase.Pages.ExistingFiles
{
    [QueryProperty(nameof(ExistingFilesM), nameof(ExistingFilesM))]
    [QueryProperty(nameof(GetTyp), nameof(GetTyp))]
    [QueryProperty(nameof(ReturnPage), nameof(ReturnPage))]
    public partial class ExistingFilesVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ExistingFilesM> existingFilesM;

        [ObservableProperty]
        string getTyp;

        public string ReturnPage { get; set; }
        public ExistingFilesVM()
        {
            ExistingFilesM ??= [];
        }

        public static PickOptions FileTypCSV()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "csv" } },
                { DevicePlatform.Android, new[] { "csv" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }
        public static PickOptions FileTypJson()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "json", "txt" } },
                { DevicePlatform.Android, new[] { "json", "txt" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }
        public static ObservableCollection<ExistingFilesM> GetExistingFiles(IList<string> values)

        {
            var response = new ObservableCollection<ExistingFilesM>();
            if (values is null)
            {
                return response;
            }
            for (int i = 0; i < values.Count; i++)
            {
                response.Add(new ExistingFilesM()
                {
                    Path = values[i],
                    Name = Path.GetFileName(values[i])
                });

            }
            return response;
        }


        [RelayCommand]
        async Task SelectedImport(ExistingFilesM filesM)
        {
            if (filesM is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"../../{ReturnPage}?FilesPath={filesM.Path}");
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

        [RelayCommand]
        async Task FindFile()
        {
            try
            {
                var response = await FilePicker.Default.PickAsync();

                if (response is not null)
                {
                    await Shell.Current.GoToAsync($"../../{ReturnPage}?FilesPath={response.FullPath}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
