using DriversRoutes.Model;

namespace DriversRoutes.Helper
{
    public static class ParseAddress
    {
        public static ResidentialAddress FromGoogleToAddress(this Result from)
        {
            var to = new ResidentialAddress();

            for (int i = 0; i < from.Address_components.Count; i++)
            {
                if (from.Address_components[i].Types.Contains("route"))
                    to.Street = from.Address_components[i].Long_name;

                if (from.Address_components[i].Types.Contains("premise"))
                    to.HouseNumber = from.Address_components[i].Long_name;

                if (from.Address_components[i].Types.Contains("subpremise"))
                    to.ApartmentNumber = from.Address_components[i].Long_name;

                if (from.Address_components[i].Types.Contains("postal_code"))
                    to.PostalCode = from.Address_components[i].Long_name;

                if (from.Address_components[i].Types.Contains("locality"))
                    to.City = from.Address_components[i].Long_name;

                if (from.Address_components[i].Types.Contains("country"))
                    to.Country = from.Address_components[i].Long_name;
            }

            return to;
        }


    }
}
