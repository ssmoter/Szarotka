using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shared.Pages.Popups.SubAddLastValue
{
    public partial class SubAddLastValueVM : ObservableObject
    {
        private SubAddLastValueM subAddLastValueM;
        public SubAddLastValueM SubAddLastValueM
        {
            get => subAddLastValueM;
            set
            {
                if (SetProperty(ref subAddLastValueM, value))
                {
                    OnPropertyChanged(nameof(SubAddLastValueM));
                }
            }
        }


        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default)
        {
            return Close?.Invoke(result, token);
        }

        public SubAddLastValueVM()
        {
            SubAddLastValueM ??= new();

        }


        [RelayCommand]
        async Task SaveAndReturn()
        {
            await OnClose(SubAddLastValueM.Result);
        }

        [RelayCommand]
        async Task CancelAndReturn()
        {
            await OnClose(null);
        }


    }
}
