using DataBase.Model.EntitiesRoutes;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (CustomerRoutes)
    public class CustomerRoutesOrdinals : BaseOrdinals
    {
        public int RoutesId { get; }
        public int Name { get; }
        public int Description { get; }
        public int PhoneNumber { get; }
        public int Longitude { get; }
        public int Latitude { get; }

        public CustomerRoutesOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOptionalOrdinal uruchamia się TYLKO RAZ przed pętlą!
            RoutesId = reader.GetOptionalOrdinal(nameof(CustomerRoutes.RoutesId));
            Name = reader.GetOptionalOrdinal(nameof(CustomerRoutes.Name));
            Description = reader.GetOptionalOrdinal(nameof(CustomerRoutes.Description));
            PhoneNumber = reader.GetOptionalOrdinal(nameof(CustomerRoutes.PhoneNumber));
            Longitude = reader.GetOptionalOrdinal(nameof(CustomerRoutes.Longitude));
            Latitude = reader.GetOptionalOrdinal(nameof(CustomerRoutes.Latitude));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla CustomerRoutes (EntitiesRoutes)
    private static void InitCustomerRoutesMappers()
    {
        _registry.Add(typeof(CustomerRoutes), (Func<SqliteDataReader, object, CustomerRoutes>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not CustomerRoutesOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for CustomerRoutes mapper", nameof(ordinalsObj));
            }

            var cr = new CustomerRoutes();

            if (ords.RoutesId >= 0 && !reader.IsDBNull(ords.RoutesId))
            {
                var rId = reader.GetString(ords.RoutesId);
                cr.RoutesId = string.IsNullOrEmpty(rId) ? Guid.Empty : new Guid(rId);
            }
            else
            {
                cr.RoutesId = Guid.Empty;
            }

            if (ords.Name >= 0 && !reader.IsDBNull(ords.Name))
                cr.Name = reader.GetString(ords.Name);
            else
                cr.Name = string.Empty;

            if (ords.Description >= 0 && !reader.IsDBNull(ords.Description))
                cr.Description = reader.GetString(ords.Description);
            else
                cr.Description = string.Empty;

            if (ords.PhoneNumber >= 0 && !reader.IsDBNull(ords.PhoneNumber))
                cr.PhoneNumber = reader.GetString(ords.PhoneNumber);
            else
                cr.PhoneNumber = string.Empty;

            if (ords.Longitude >= 0 && !reader.IsDBNull(ords.Longitude))
                cr.Longitude = reader.GetDouble(ords.Longitude);
            else
                cr.Longitude = 0d;

            if (ords.Latitude >= 0 && !reader.IsDBNull(ords.Latitude))
                cr.Latitude = reader.GetDouble(ords.Latitude);
            else
                cr.Latitude = 0d;

            MapBaseFields(reader, cr, ords);

            return cr;
        }));
    }
}
