using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DataBase.Pages.Popups.SubAddLastValue
{
    public partial class SubAddLastValueVM : ObservableObject
    {
        [ObservableProperty]
        SubAddLastValueM subAddLastValueM;


        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
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
