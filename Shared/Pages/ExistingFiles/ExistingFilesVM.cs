using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace Shared.Pages.ExistingFiles
{
    public partial class ExistingFilesVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(ExistingFilesM), out object existingFilesM))
            {
                if (existingFilesM is ObservableCollection<ExistingFilesM> collection)
                {
                    ExistingFilesM = collection;
                }
            }
            if (query.TryGetValue(nameof(GetTyp), out object getTyp))
            {
                if (getTyp is string _getType)
                {
                    GetTyp = _getType;
                }
            }
            if (query.TryGetValue(nameof(ReturnPage), out object returnPage))
            {
                if (returnPage is string _returnPage)
                {
                    ReturnPage = _returnPage;
                }
            }
        }

        private ObservableCollection<ExistingFilesM> existingFilesM;
        public ObservableCollection<ExistingFilesM> ExistingFilesM
        {
            get => existingFilesM;
            set
            {
                if (SetProperty(ref existingFilesM, value))
                {
                    OnPropertyChanged(nameof(ExistingFilesM));
                }
            }
        }

        private string getTyp = "";
        public string GetTyp
        {
            get => getTyp;
            set
            {
                if (SetProperty(ref getTyp, value))
                {
                    OnPropertyChanged(nameof(GetTyp));
                }
            }
        }

        public string ReturnPage { get; set; } = "";
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

            //ExistingFilesVM.FileTypJson()
            var response = await FilePicker.PickAsync();

            if (response is not null)
            {
                GoAndForget(response);
            }
        }

        private async void GoAndForget(FileResult response)
        {
            await Shell.Current.GoToAsync($"../../{ReturnPage}?FilesPath={response.FullPath}");
        }


    }
}
