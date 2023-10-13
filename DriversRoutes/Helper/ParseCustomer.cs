using DriversRoutes.Model;
using DriversRoutes.Pages.Maps;

namespace DriversRoutes.Helper
{
    public static class ParseCustomer
    {
        public static void ParseAsCustomerM(this CustomerRoutes from, MapsM to)
        {
            if (from is null)
            {
                return;
            }
            to ??= new();

            to.Id = from.Id;
            to.RoutesId = from.RoutesId;
            to.Index = from.QueueNumber;
            to.Name = from.Name;
            to.Description = from.Description;
            to.PhoneNumber = from.PhoneNumber;
            to.Created = from.CreatedDate;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.SelectedDayOfWeek = from.DayOfWeek;
            to.SetPin();
            from.Dispose();
        }
        public static void ParseAsCustomer(this MapsM from, CustomerRoutes to)
        {
            if (from is null)
            {
                return;
            }
            to ??= new();

            to.Id = from.Id;
            to.RoutesId = from.RoutesId;
            to.QueueNumber = from.Index;
            to.Name = from.Name;
            to.Description = from.Description;
            to.PhoneNumber = from.PhoneNumber;
            to.CreatedDate = from.Created;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.DayOfWeek = from.SelectedDayOfWeek;
        }


        public static MapsM ParseAsCustomerM(this CustomerRoutes from)
        {
            if (from is null)
            {
                return null;
            }
            MapsM to = new();

            to.Id = from.Id;
            to.RoutesId = from.RoutesId;
            to.Index = from.QueueNumber;
            to.Name = from.Name;
            to.Description = from.Description;
            to.PhoneNumber = from.PhoneNumber;
            to.Created = from.CreatedDate;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.SelectedDayOfWeek = from.DayOfWeek;
            to.SetPin();
            from.Dispose();
            return to;
        }
        public static CustomerRoutes ParseAsCustomer(this MapsM from)
        {
            if (from is null)
            {
                return null;
            }
            CustomerRoutes to = new();

            to.Id = from.Id;
            to.RoutesId = from.RoutesId;
            to.QueueNumber = from.Index;
            to.Name = from.Name;
            to.Description = from.Description;
            to.PhoneNumber = from.PhoneNumber;
            to.CreatedDate = from.Created;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.DayOfWeek = from.SelectedDayOfWeek;
            return to;
        }

    }
}
