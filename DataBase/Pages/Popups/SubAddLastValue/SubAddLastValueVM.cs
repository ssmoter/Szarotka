using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DataBase.Pages.Popups.SubAddLastValue
{
    public partial class SubAddLastValueVM : ObservableObject
    {
        [ObservableProperty]
        SubAddLastValueM subAddLastValueM;


        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default(CancellationToken))
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
