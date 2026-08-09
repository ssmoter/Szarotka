using DataBase.Model.EntitiesRoutes;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (ResidentialAddress)
    public class ResidentialAddressOrdinals : BaseOrdinals
    {
        public int CustomerId { get; }
        public int Name { get; }
        public int Surname { get; }
        public int Street { get; }
        public int HouseNumber { get; }
        public int ApartmentNumber { get; }
        public int PostalCode { get; }
        public int City { get; }
        public int Country { get; }

        public ResidentialAddressOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            CustomerId = reader.GetOrdinal(nameof(ResidentialAddress.CustomerId));
            Name = reader.GetOrdinal(nameof(ResidentialAddress.Name));
            Surname = reader.GetOrdinal(nameof(ResidentialAddress.Surname));
            Street = reader.GetOrdinal(nameof(ResidentialAddress.Street));
            HouseNumber = reader.GetOrdinal(nameof(ResidentialAddress.HouseNumber));
            ApartmentNumber = reader.GetOrdinal(nameof(ResidentialAddress.ApartmentNumber));
            PostalCode = reader.GetOrdinal(nameof(ResidentialAddress.PostalCode));
            City = reader.GetOrdinal(nameof(ResidentialAddress.City));
            Country = reader.GetOrdinal(nameof(ResidentialAddress.Country));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla ResidentialAddress (EntitiesRoutes)
    private static void InitResidentialAddressMappers()
    {
        _registry.Add(typeof(ResidentialAddress), (Func<SqliteDataReader, object, ResidentialAddress>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ResidentialAddressOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for ResidentialAddress mapper", nameof(ordinalsObj));
            }

            var ra = new ResidentialAddress();

            if (!reader.IsDBNull(ords.CustomerId))
            {
                var cId = reader.GetString(ords.CustomerId);
                ra.CustomerId = string.IsNullOrEmpty(cId) ? Guid.Empty : new Guid(cId);
            }
            else
            {
                ra.CustomerId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.Name)) ra.Name = reader.GetString(ords.Name); else ra.Name = string.Empty;
            if (!reader.IsDBNull(ords.Surname)) ra.Surname = reader.GetString(ords.Surname); else ra.Surname = string.Empty;
            if (!reader.IsDBNull(ords.Street)) ra.Street = reader.GetString(ords.Street); else ra.Street = string.Empty;
            if (!reader.IsDBNull(ords.HouseNumber)) ra.HouseNumber = reader.GetString(ords.HouseNumber); else ra.HouseNumber = string.Empty;
            if (!reader.IsDBNull(ords.ApartmentNumber)) ra.ApartmentNumber = reader.GetString(ords.ApartmentNumber); else ra.ApartmentNumber = string.Empty;
            if (!reader.IsDBNull(ords.PostalCode)) ra.PostalCode = reader.GetString(ords.PostalCode); else ra.PostalCode = string.Empty;
            if (!reader.IsDBNull(ords.City)) ra.City = reader.GetString(ords.City); else ra.City = string.Empty;
            if (!reader.IsDBNull(ords.Country)) ra.Country = reader.GetString(ords.Country); else ra.Country = string.Empty;

            MapBaseFields(reader, ra, ords);

            return ra;
        }));
    }
}
