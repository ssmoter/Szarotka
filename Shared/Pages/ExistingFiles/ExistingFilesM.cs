using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.ExistingFiles
{
    public partial class ExistingFilesM : ObservableObject
    {

        private string path;
        public string Path
        {
            get => path;
            set
            {
                if (SetProperty(ref path, value, nameof(Path)))
                {
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name)))
                {
                }
            }
        }

    }
}
