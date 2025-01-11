using CommunityToolkit.Mvvm.ComponentModel;

namespace DriversRoutes.Pages.Maps.MapSmall
{
    public partial class MapSmallM : ObservableObject
    {
        private bool changeLocation = true;
        public bool ChangeLocation
        {
            get => changeLocation;
            set
            {
                if (SetProperty(ref changeLocation, value, nameof(ChangeLocation))) { }
            }
        }
        private bool saveLocation = false;
        public bool SaveLocation
        {
            get => saveLocation;
            set
            {
                if (SetProperty(ref saveLocation, value, nameof(SaveLocation))) { }
            }
        }


        public MapSmallM()
        {
        }

        public double OldLongitude { get; set; }
        public double OldLatitude { get; set; }

    }
}
