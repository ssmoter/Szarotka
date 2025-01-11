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
                if (SetProperty(ref date, value, nameof(Date)))
                {
                    //OnPropertyChanged(nameof(Date));
                    DisplayDate = Date.ToString("dd.MM.yyyy");
                }
            }
        }

        private string displayDate;
        public string DisplayDate
        {
            get => displayDate;
            set
            {
                if (SetProperty(ref displayDate, value, nameof(DisplayDate))) { }
            }
        }

        public MainM()
        {
            Date = DateTime.Now;
        }

    }
}
