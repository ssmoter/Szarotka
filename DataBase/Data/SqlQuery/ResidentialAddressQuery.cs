using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.SqlQuery
{
    public class ResidentialAddressQuery
    {
        public static string SaveOrUpdate(
            Guid Id,
            Guid CustomerId,
            string Name,
            string Surname,
            string Street,
            string HouseNumber,
            string ApartmentNumber,
            string PostalCode,
            string City,
            string Country,
            long CreatedTicks,
            long UpdatedTicks,
            bool IsDelete,
            Guid UserCreatedId,
            Guid UserUpdatedId)
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
                            @{nameof(Id)}, 
                            @{nameof(CustomerId)}, 
                            @{nameof(Name)}, 
                            @{nameof(Surname)}, 
                            @{nameof(Street)}, 
                            @{nameof(HouseNumber)}, 
                            @{nameof(ApartmentNumber)}, 
                            @{nameof(PostalCode)}, 
                            @{nameof(City)}, 
                            @{nameof(Country)}, 
                            @{nameof(CreatedTicks)}, 
                            @{nameof(UpdatedTicks)}, 
                            @{nameof(IsDelete)}, 
                            @{nameof(UserCreatedId)}, 
                            @{nameof(UserUpdatedId)}
                        ) ON CONFLICT({nameof(ResidentialAddress.Id)}) DO UPDATE SET
                            {nameof(ResidentialAddress.CustomerId)} = @{nameof(CustomerId)},
                            {nameof(ResidentialAddress.Name)} = @{nameof(Name)},
                            {nameof(ResidentialAddress.Surname)} = @{nameof(Surname)},
                            {nameof(ResidentialAddress.Street)} = @{nameof(Street)},
                            {nameof(ResidentialAddress.HouseNumber)} = @{nameof(HouseNumber)},
                            {nameof(ResidentialAddress.ApartmentNumber)} = @{nameof(ApartmentNumber)},
                            {nameof(ResidentialAddress.PostalCode)} = @{nameof(PostalCode)},
                            {nameof(ResidentialAddress.City)} = @{nameof(City)},
                            {nameof(ResidentialAddress.Country)} = @{nameof(Country)},
                            {nameof(ResidentialAddress.UpdatedTicks)} = @{nameof(UpdatedTicks)},
                            {nameof(ResidentialAddress.IsDelete)} = @{nameof(IsDelete)},
                            {nameof(ResidentialAddress.UserUpdatedId)} = @{nameof(UserUpdatedId)};
                    ";
            return sql;
        }
    }
}
