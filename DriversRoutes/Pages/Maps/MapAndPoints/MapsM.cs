using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesRoutes;

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps.MapAndPoints
{
    public partial class MapsM : ObservableObject
    {
        [ObservableProperty]
        DriversRoutes.Model.CustomPin pin;

        [ObservableProperty]
        CustomerRoutes customerRoutes;

        public MapsM()
        {
            Pin = new DriversRoutes.Model.CustomPin();
            CustomerRoutes ??= new CustomerRoutes();
        }

        public void SetPin()
        {
            Pin ??= new DriversRoutes.Model.CustomPin();

            Pin.Location = new Location(CustomerRoutes.Latitude, CustomerRoutes.Longitude);
            Pin.Label = $"{CustomerRoutes.QueueNumber}: {CustomerRoutes.Name}";
            Pin.Address = CustomerRoutes.Description;
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
            CustomerRoutes.QueueNumber = i;
            CustomerRoutes.Id = Guid.NewGuid();
            CustomerRoutes.Name = RandomString(i);
            CustomerRoutes.Description = RandomString(i * 2);
            //49.7488002173044, 20.408379427432106
            CustomerRoutes.Latitude = 49.7488002173044 + Random.Shared.NextDouble();
            CustomerRoutes.Longitude = 20.408379427432106 + Random.Shared.NextDouble();
            CustomerRoutes.DayOfWeek = RandomDay(i);
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
