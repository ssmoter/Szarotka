using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.Main
{
    public partial class MainM : ObservableObject
    {

        DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (SetProperty(ref date, value))
                {
                    OnPropertyChanged(nameof(Date));
                    DisplyDate = Date.ToString("dd.MM.yyyy");
                }
            }
        }

        [ObservableProperty]
        string displyDate;

        public MainM()
        {
            Date = DateTime.Now;
        }

    }
}
