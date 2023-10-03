using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Maui.Controls.Maps;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Maps
{
    public partial class MapsM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Pin> pins;

        public MapsM()
        {
            Pins= new ObservableCollection<Pin>();
        }

    }
}
