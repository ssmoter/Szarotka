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
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            RoutesId = reader.GetOrdinal(nameof(CustomerRoutes.RoutesId));
            Name = reader.GetOrdinal(nameof(CustomerRoutes.Name));
            Description = reader.GetOrdinal(nameof(CustomerRoutes.Description));
            PhoneNumber = reader.GetOrdinal(nameof(CustomerRoutes.PhoneNumber));
            Longitude = reader.GetOrdinal(nameof(CustomerRoutes.Longitude));
            Latitude = reader.GetOrdinal(nameof(CustomerRoutes.Latitude));
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

            if (!reader.IsDBNull(ords.RoutesId))
            {
                var rId = reader.GetString(ords.RoutesId);
                cr.RoutesId = string.IsNullOrEmpty(rId) ? Guid.Empty : new Guid(rId);
            }
            else
            {
                cr.RoutesId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.Name))
                cr.Name = reader.GetString(ords.Name);
            else
                cr.Name = string.Empty;

            if (!reader.IsDBNull(ords.Description))
                cr.Description = reader.GetString(ords.Description);
            else
                cr.Description = string.Empty;

            if (!reader.IsDBNull(ords.PhoneNumber))
                cr.PhoneNumber = reader.GetString(ords.PhoneNumber);
            else
                cr.PhoneNumber = string.Empty;

            if (!reader.IsDBNull(ords.Longitude))
                cr.Longitude = reader.GetDouble(ords.Longitude);
            else
                cr.Longitude = 0d;

            if (!reader.IsDBNull(ords.Latitude))
                cr.Latitude = reader.GetDouble(ords.Latitude);
            else
                cr.Latitude = 0d;

            MapBaseFields(reader, cr, ords);

            return cr;
        }));
    }
}
