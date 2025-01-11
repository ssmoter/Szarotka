using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace Inventory.Pages.PopupCalculator
{
    public partial class PopupCalculatorM : ObservableObject
    {
        private ObservableCollection<decimal> values;
        public ObservableCollection<decimal> Values
        {
            get => values;
            set
            {
                if (SetProperty(ref values, value, nameof(Values))) { }
            }
        }

        private ObservableCollection<char> sigs;
        public ObservableCollection<char> Sigs
        {
            get => sigs;
            set
            {
                if (SetProperty(ref sigs, value, nameof(Sigs))) { }
            }
        }

        public PopupCalculatorM()
        {
            Values = [];
            Sigs = [];
        }

    }
}
