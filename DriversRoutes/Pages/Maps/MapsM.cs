using CommunityToolkit.Mvvm.ComponentModel;

#if ANDROID
using DriversRoutes.Platforms.Android;
#endif

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps
{
    public partial class MapsM : ObservableObject
    {
        //#if ANDROID

        //        [ObservableProperty]
        //        CustomPin pin;
        //#else
        //#endif
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

        [ObservableProperty]
        ImageSource imageSource;

        public MapsM()
        {
#if ANDROID
            Pin = new CustomPin();
#else
            Pin = new Pin();
#endif

            SelectedDayOfWeek ??= new Model.SelectedDayOfWeek();
        }

        public void SetPin()
        {
            Pin = new Pin();
            Pin.Location = new Location(Latitude, Longitude);
            Pin.Label = $"{Index}: {Name}";
            Pin.Address = Description;
            Pin.Type = PinType.SavedPin;
        }

        public MapsM CreateRandomPoint(int i)
        {
            Index = i;
            Name = RandomString(Index);
            Description = RandomString(Index * 2);
            //49.7488002173044, 20.408379427432106
            Latitude = 49.7488002173044 + Random.Shared.NextDouble();
            Longitude = 20.408379427432106 + Random.Shared.NextDouble();


            //#if ANDROID
            //            Pin = new CustomPin();
            //            Pin.ImageSource = ImageSource.FromFile(DataBase.Helper.Img.ImgPath.Logo);
            SetPin();
            //#else
            //#endif


            return this;
        }

        private static Random random = new Random();
        static string RandomString(int length)
        {
            const string chars = "ABC DEFGH IJKLMN OPQRS TUVWXYZ 01234 56789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
