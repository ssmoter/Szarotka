using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace Inventory.Pages.PopupCalculator
{
    public partial class PopupCalculatorM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<decimal> values;

        [ObservableProperty]
        ObservableCollection<char> sigs;


        public PopupCalculatorM()
        {
            Values = new ObservableCollection<decimal>();
            Sigs = new ObservableCollection<char>();
        }

    }
}
