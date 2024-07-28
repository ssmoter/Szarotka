using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesRoutes;


#if ANDROID
using DriversRoutes.Platforms.Android;
#endif

using Microsoft.Maui.Controls.Maps;

using SQLite;

namespace DriversRoutes.Pages.Maps.MapAndPoints
{
    public partial class MapsM : ObservableObject
    {
        [ObservableProperty]
        DriversRoutes.Model.CustomPin pin;
#if ANDROID
#else
        //[ObservableProperty]
       // Pin pin;
#endif

        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        Guid routesId;

        int index;
        [Ignore]
        public int Index
        {
            get => index;
            set
            {
                if (SetProperty(ref index, value))
                {
                    OnPropertyChanged(nameof(Index));
                }
            }
        }

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
        SelectedDayOfWeekRoutes selectedDayOfWeek;
        [ObservableProperty]
        ResidentialAddress residentialAddress;

        [ObservableProperty]
        ImageSource imageSource;

        public MapsM()
        {
            Pin = new DriversRoutes.Model.CustomPin();
#if ANDROID
#else
            //Pin = new Pin();
#endif

            SelectedDayOfWeek ??= new SelectedDayOfWeekRoutes();
        }

        public void SetPin()
        {
            Pin = new DriversRoutes.Model.CustomPin();
#if ANDROID
#else
            //Pin = new Pin();
#endif

            Pin.Location = new Location(Latitude, Longitude);
            Pin.Label = $"{Index}: {Name}";
            Pin.Address = Description;
            Pin.Type = PinType.SavedPin;
        }

        const char colonChar = ':';
        public static int GetIndex(ReadOnlySpan<char> chars)
        {
            int index = -1;
            if (chars.IsEmpty)
            {
                return index;
            }

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == colonChar)
                {
                    index = int.Parse(chars[..i]);
                    break;
                }
            }
            return index;
        }


        public MapsM CreateRandomPoint(int i)
        {
            Index = i;
            Id = Guid.NewGuid();
            Name = RandomString(Index);
            Description = RandomString(Index * 2);
            //49.7488002173044, 20.408379427432106
            Latitude = 49.7488002173044 + Random.Shared.NextDouble();
            Longitude = 20.408379427432106 + Random.Shared.NextDouble();
            SelectedDayOfWeek = RandomDay(Index);
            SetPin();

            return this;
        }

        private static readonly Random random = new();
        static string RandomString(int length)
        {
            const string chars = "ABC DEFGH IJKLMN OPQRS TUVWXYZ 01234 56789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static SelectedDayOfWeekRoutes RandomDay(int length)
        {
            var day = new SelectedDayOfWeekRoutes();

            if (length % 2 == 0)
                day.Sunday = true;
            if (length % 3 == 0)
                day.Monday = true;
            if (length % 4 == 0)
                day.Tuesday = true;
            if (length % 5 == 0)
                day.Wednesday = true;
            if (length % 6 == 0)
                day.Thursday = true;
            if (length % 7 == 0)
                day.Friday = true;
            if (length % 8 == 0)
                day.Saturday = true;

            return day;
        }

    }
}
