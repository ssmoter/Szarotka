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
            to.CustomerRoutes ??= new();

            to.CustomerRoutes.Id = from.Id;
            to.CustomerRoutes.RoutesId = from.RoutesId;
            to.CustomerRoutes.QueueNumber = from.QueueNumber;
            to.CustomerRoutes.Name = from.Name;
            to.CustomerRoutes.Description = from.Description;
            to.CustomerRoutes.PhoneNumber = from.PhoneNumber;
            to.CustomerRoutes.Created = from.Created;
            to.CustomerRoutes.Longitude = from.Longitude;
            to.CustomerRoutes.Latitude = from.Latitude;
            to.CustomerRoutes.DayOfWeek = from.DayOfWeek;
            to.CustomerRoutes.ResidentialAddress = from.ResidentialAddress;
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

            to.Id = from.CustomerRoutes.Id;
            to.RoutesId = from.CustomerRoutes.RoutesId;
            to.QueueNumber = from.CustomerRoutes.QueueNumber;
            to.Name = from.CustomerRoutes.Name;
            to.Description = from.CustomerRoutes.Description;
            to.PhoneNumber = from.CustomerRoutes.PhoneNumber;
            to.Created = from.CustomerRoutes.Created;
            to.Longitude = from.CustomerRoutes.Longitude;
            to.Latitude = from.CustomerRoutes.Latitude;
            to.DayOfWeek = from.CustomerRoutes.DayOfWeek;
            to.ResidentialAddress = from.CustomerRoutes.ResidentialAddress;
        }


        public static MapsM ParseAsCustomerM(this CustomerRoutes from)
        {
            if (from is null)
            {
                return null;
            }
            MapsM to = new()
            {
                CustomerRoutes = new()
                {
                    Id = from.Id,
                    RoutesId = from.RoutesId,
                    QueueNumber = from.QueueNumber,
                    Name = from.Name,
                    Description = from.Description,
                    PhoneNumber = from.PhoneNumber,
                    Created = from.Created,
                    Longitude = from.Longitude,
                    Latitude = from.Latitude,
                    DayOfWeek = from.DayOfWeek,
                    ResidentialAddress = from.ResidentialAddress,
                }
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
                Id = from.CustomerRoutes.Id,
                RoutesId = from.CustomerRoutes.RoutesId,
                QueueNumber = from.CustomerRoutes.QueueNumber,
                Name = from.CustomerRoutes.Name,
                Description = from.CustomerRoutes.Description,
                PhoneNumber = from.CustomerRoutes.PhoneNumber,
                Created = from.CustomerRoutes.Created,
                Longitude = from.CustomerRoutes.Longitude,
                Latitude = from.CustomerRoutes.Latitude,
                DayOfWeek = from.CustomerRoutes.DayOfWeek,
                ResidentialAddress = from.CustomerRoutes.ResidentialAddress
            };
            return to;
        }

    }
}
