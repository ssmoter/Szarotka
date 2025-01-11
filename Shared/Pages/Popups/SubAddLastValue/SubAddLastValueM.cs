using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.Popups.SubAddLastValue
{
    public partial class SubAddLastValueM : ObservableObject
    {
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (SetProperty(ref title, value))
                {
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private int oldValue;
        public int OldValue
        {
            get => oldValue;
            set
            {
                if (SetProperty(ref oldValue, value))
                {
                    OnPropertyChanged(nameof(OldValue));
                }
            }
        }
        private int newValue;
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


        private int result;
        public int Result
        {
            get => result;
            set
            {
                if (SetProperty(ref result, value))
                {
                    OnPropertyChanged(nameof(Result));
                }
            }
        }

    }
}
