using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Pages.Popups.SubAddLastValue
{
    public partial class SubAddLastValueM : ObservableObject
    {
        [ObservableProperty]
        string title;

        [ObservableProperty]
        int oldValue;
        int newValue;
        public int NewValue
        {
            get => newValue;
            set
            {
                if (SetProperty(ref newValue, value))
                {
                    OnPropertyChanged(nameof(NewValue));
                    Result = OldValue + NewValue;
                }
            }
        }


        [ObservableProperty]
        int result;

    }
}
