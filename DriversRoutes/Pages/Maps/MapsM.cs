using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps
{
    public partial class MapsM : ObservableObject
    {
        [ObservableProperty]
        Pin pin;

        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        Guid routesId;

        [ObservableProperty]
        int index;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string phoneNumber;

        [ObservableProperty]
        DateTime created;

        [ObservableProperty]
        double longitude;

        [ObservableProperty]
        double latitude;

        [ObservableProperty]
        Model.SelectedDayOfWeek selectedDayOfWeek;

        public MapsM()
        {
            Pin ??= new Pin();
            SelectedDayOfWeek ??= new Model.SelectedDayOfWeek();
        }
        public MapsM CreateRandomPoint(int i)
        {
            Index = i;
            Name = RandomString(Index);
            Description = RandomString(Index * 2);
            //49.7488002173044, 20.408379427432106
            Latitude = 49.7488002173044 + Random.Shared.NextDouble();
            Longitude = 20.408379427432106 + Random.Shared.NextDouble();


            Pin ??= new Pin();
            Pin.Location = new Location(Latitude, Longitude);
            Pin.Label = $"{Index}: {Name}";
            Pin.Address = Description;
            Pin.Type = PinType.Generic;

            return this;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABC DEFGH IJKLMN OPQRS TUVWXYZ 01234 56789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
