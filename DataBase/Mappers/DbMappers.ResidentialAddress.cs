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
            // Tutaj GetOptionalOrdinal uruchamia się TYLKO RAZ przed pętlą!
            CustomerId = reader.GetOptionalOrdinal(nameof(ResidentialAddress.CustomerId));
            Name = reader.GetOptionalOrdinal(nameof(ResidentialAddress.Name));
            Surname = reader.GetOptionalOrdinal(nameof(ResidentialAddress.Surname));
            Street = reader.GetOptionalOrdinal(nameof(ResidentialAddress.Street));
            HouseNumber = reader.GetOptionalOrdinal(nameof(ResidentialAddress.HouseNumber));
            ApartmentNumber = reader.GetOptionalOrdinal(nameof(ResidentialAddress.ApartmentNumber));
            PostalCode = reader.GetOptionalOrdinal(nameof(ResidentialAddress.PostalCode));
            City = reader.GetOptionalOrdinal(nameof(ResidentialAddress.City));
            Country = reader.GetOptionalOrdinal(nameof(ResidentialAddress.Country));
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

            if (ords.CustomerId >= 0 && !reader.IsDBNull(ords.CustomerId))
            {
                var cId = reader.GetString(ords.CustomerId);
                ra.CustomerId = string.IsNullOrEmpty(cId) ? Guid.Empty : new Guid(cId);
            }
            else
            {
                ra.CustomerId = Guid.Empty;
            }

            if (ords.Name >= 0 && !reader.IsDBNull(ords.Name)) ra.Name = reader.GetString(ords.Name); else ra.Name = string.Empty;
            if (ords.Surname >= 0 && !reader.IsDBNull(ords.Surname)) ra.Surname = reader.GetString(ords.Surname); else ra.Surname = string.Empty;
            if (ords.Street >= 0 && !reader.IsDBNull(ords.Street)) ra.Street = reader.GetString(ords.Street); else ra.Street = string.Empty;
            if (ords.HouseNumber >= 0 && !reader.IsDBNull(ords.HouseNumber)) ra.HouseNumber = reader.GetString(ords.HouseNumber); else ra.HouseNumber = string.Empty;
            if (ords.ApartmentNumber >= 0 && !reader.IsDBNull(ords.ApartmentNumber)) ra.ApartmentNumber = reader.GetString(ords.ApartmentNumber); else ra.ApartmentNumber = string.Empty;
            if (ords.PostalCode >= 0 && !reader.IsDBNull(ords.PostalCode)) ra.PostalCode = reader.GetString(ords.PostalCode); else ra.PostalCode = string.Empty;
            if (ords.City >= 0 && !reader.IsDBNull(ords.City)) ra.City = reader.GetString(ords.City); else ra.City = string.Empty;
            if (ords.Country >= 0 && !reader.IsDBNull(ords.Country)) ra.Country = reader.GetString(ords.Country); else ra.Country = string.Empty;

            MapBaseFields(reader, ra, ords);

            return ra;
        }));
    }
}
