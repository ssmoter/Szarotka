using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Szarotka.Pages.Options.Main
{
    public partial class MainOptionsVM : ObservableObject
    {
        [RelayCommand]
        async Task GoToLogs()
        {
            await Shell.Current.GoToAsync(nameof(DataBase.Pages.Log.LogV));
        }

    }
}
