using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Pages.Maps.MapAndPoints;

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
            to.Created = from.Created;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.SelectedDayOfWeek = from.DayOfWeek;
            to.ResidentialAddress = from.ResidentialAddress;
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
            to.Created = from.Created;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.DayOfWeek = from.SelectedDayOfWeek;
            to.ResidentialAddress = from.ResidentialAddress;
        }


        public static MapsM ParseAsCustomerM(this CustomerRoutes from)
        {
            if (from is null)
            {
                return null;
            }
            MapsM to = new()
            {
                Id = from.Id,
                RoutesId = from.RoutesId,
                Index = from.QueueNumber,
                Name = from.Name,
                Description = from.Description,
                PhoneNumber = from.PhoneNumber,
                Created = from.Created,
                Longitude = from.Longitude,
                Latitude = from.Latitude,
                SelectedDayOfWeek = from.DayOfWeek,
                ResidentialAddress = from.ResidentialAddress,                
            };
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
            CustomerRoutes to = new()
            {
                Id = from.Id,
                RoutesId = from.RoutesId,
                QueueNumber = from.Index,
                Name = from.Name,
                Description = from.Description,
                PhoneNumber = from.PhoneNumber,
                Created = from.Created,
                Longitude = from.Longitude,
                Latitude = from.Latitude,
                DayOfWeek = from.SelectedDayOfWeek,
                ResidentialAddress = from.ResidentialAddress
            };
            return to;
        }

    }
}
