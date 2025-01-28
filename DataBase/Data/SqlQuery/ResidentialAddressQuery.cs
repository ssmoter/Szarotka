using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class ResidentialAddressQuery
    {
        public static string SaveOrUpdate(ResidentialAddress residentialAddress)
        {
            string sql = $@"
                    INSERT INTO {nameof(ResidentialAddress)} (
                        {nameof(ResidentialAddress.Id)}, 
                        {nameof(ResidentialAddress.CustomerId)}, 
                        {nameof(ResidentialAddress.Name)}, 
                        {nameof(ResidentialAddress.Surname)}, 
                        {nameof(ResidentialAddress.Street)}, 
                        {nameof(ResidentialAddress.HouseNumber)}, 
                        {nameof(ResidentialAddress.ApartmentNumber)}, 
                        {nameof(ResidentialAddress.PostalCode)}, 
                        {nameof(ResidentialAddress.City)}, 
                        {nameof(ResidentialAddress.Country)}, 
                        {nameof(ResidentialAddress.CreatedTicks)}, 
                        {nameof(ResidentialAddress.UpdatedTicks)}, 
                        {nameof(ResidentialAddress.IsDelete)}, 
                        {nameof(ResidentialAddress.UserCreatedId)}, 
                        {nameof(ResidentialAddress.UserUpdatedId)}
                    ) VALUES (
                        '{residentialAddress.Id}', 
                        '{residentialAddress.CustomerId}', 
                        '{residentialAddress.Name}', 
                        '{residentialAddress.Surname}', 
                        '{residentialAddress.Street}', 
                        '{residentialAddress.HouseNumber}', 
                        '{residentialAddress.ApartmentNumber}', 
                        '{residentialAddress.PostalCode}', 
                        '{residentialAddress.City}', 
                        '{residentialAddress.Country}', 
                        {residentialAddress.CreatedTicks}, 
                        {residentialAddress.UpdatedTicks}, 
                        {residentialAddress.IsDelete}, 
                        '{residentialAddress.UserCreatedId}', 
                        '{residentialAddress.UserUpdatedId}'
                    ) ON CONFLICT({nameof(ResidentialAddress.Id)}) DO UPDATE SET
                        {nameof(ResidentialAddress.CustomerId)} = '{residentialAddress.CustomerId}',
                        {nameof(ResidentialAddress.Name)} = '{residentialAddress.Name}',
                        {nameof(ResidentialAddress.Surname)} = '{residentialAddress.Surname}',
                        {nameof(ResidentialAddress.Street)} = '{residentialAddress.Street}',
                        {nameof(ResidentialAddress.HouseNumber)} = '{residentialAddress.HouseNumber}',
                        {nameof(ResidentialAddress.ApartmentNumber)} = '{residentialAddress.ApartmentNumber}',
                        {nameof(ResidentialAddress.PostalCode)} = '{residentialAddress.PostalCode}',
                        {nameof(ResidentialAddress.City)} = '{residentialAddress.City}',
                        {nameof(ResidentialAddress.Country)} = '{residentialAddress.Country}',
                        {nameof(ResidentialAddress.UpdatedTicks)} = {residentialAddress.UpdatedTicks},
                        {nameof(ResidentialAddress.IsDelete)} = {residentialAddress.IsDelete},
                        {nameof(ResidentialAddress.UserUpdatedId)} = '{residentialAddress.UserUpdatedId}';
                ";
            return sql;
        }
    }
}
