using DataBase.Model.EntitiesRoutes;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // Struktura indeksów kolumn dla SelectedDayOfWeekRoutes
    public class SelectedDayOfWeekOrdinals : BaseOrdinals
    {
        public int CustomerId { get; }

        public int Sunday { get; }
        public int SundayTicks { get; }

        public int Monday { get; }
        public int MondayTicks { get; }

        public int Tuesday { get; }
        public int TuesdayTicks { get; }

        public int Wednesday { get; }
        public int WednesdayTicks { get; }

        public int Thursday { get; }
        public int ThursdayTicks { get; }

        public int Friday { get; }
        public int FridayTicks { get; }

        public int Saturday { get; }
        public int SaturdayTicks { get; }

        public SelectedDayOfWeekOrdinals(SqliteDataReader reader) : base(reader)
        {
            CustomerId = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.CustomerId));

            Sunday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Sunday));
            SundayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.SundayTicks));

            Monday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Monday));
            MondayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.MondayTicks));

            Tuesday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Tuesday));
            TuesdayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.TuesdayTicks));

            Wednesday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Wednesday));
            WednesdayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.WednesdayTicks));

            Thursday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Thursday));
            ThursdayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.ThursdayTicks));

            Friday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Friday));
            FridayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.FridayTicks));

            Saturday = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.Saturday));
            SaturdayTicks = reader.GetOrdinal(nameof(SelectedDayOfWeekRoutes.SaturdayTicks));
        }
    }

    private static void InitSelectedDayOfWeekMappers()
    {
        _registry.Add(typeof(SelectedDayOfWeekRoutes), (Func<SqliteDataReader, object, SelectedDayOfWeekRoutes>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not SelectedDayOfWeekOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for SelectedDayOfWeekRoutes mapper", nameof(ordinalsObj));

            var sd = new SelectedDayOfWeekRoutes();

            if (!reader.IsDBNull(ords.CustomerId))
            {
                var s = reader.GetString(ords.CustomerId);
                sd.CustomerId = string.IsNullOrEmpty(s) ? Guid.Empty : new Guid(s);
            }
            else sd.CustomerId = Guid.Empty;

            if (!reader.IsDBNull(ords.Sunday)) sd.Sunday = reader.GetInt32(ords.Sunday) == 1; else sd.Sunday = false;
            if (!reader.IsDBNull(ords.SundayTicks)) sd.SundayTicks = reader.GetInt64(ords.SundayTicks); else sd.SundayTicks = 0L;

            if (!reader.IsDBNull(ords.Monday)) sd.Monday = reader.GetInt32(ords.Monday) == 1; else sd.Monday = false;
            if (!reader.IsDBNull(ords.MondayTicks)) sd.MondayTicks = reader.GetInt64(ords.MondayTicks); else sd.MondayTicks = 0L;

            if (!reader.IsDBNull(ords.Tuesday)) sd.Tuesday = reader.GetInt32(ords.Tuesday) == 1; else sd.Tuesday = false;
            if (!reader.IsDBNull(ords.TuesdayTicks)) sd.TuesdayTicks = reader.GetInt64(ords.TuesdayTicks); else sd.TuesdayTicks = 0L;

            if (!reader.IsDBNull(ords.Wednesday)) sd.Wednesday = reader.GetInt32(ords.Wednesday) == 1; else sd.Wednesday = false;
            if (!reader.IsDBNull(ords.WednesdayTicks)) sd.WednesdayTicks = reader.GetInt64(ords.WednesdayTicks); else sd.WednesdayTicks = 0L;

            if (!reader.IsDBNull(ords.Thursday)) sd.Thursday = reader.GetInt32(ords.Thursday) == 1; else sd.Thursday = false;
            if (!reader.IsDBNull(ords.ThursdayTicks)) sd.ThursdayTicks = reader.GetInt64(ords.ThursdayTicks); else sd.ThursdayTicks = 0L;

            if (!reader.IsDBNull(ords.Friday)) sd.Friday = reader.GetInt32(ords.Friday) == 1; else sd.Friday = false;
            if (!reader.IsDBNull(ords.FridayTicks)) sd.FridayTicks = reader.GetInt64(ords.FridayTicks); else sd.FridayTicks = 0L;

            if (!reader.IsDBNull(ords.Saturday)) sd.Saturday = reader.GetInt32(ords.Saturday) == 1; else sd.Saturday = false;
            if (!reader.IsDBNull(ords.SaturdayTicks)) sd.SaturdayTicks = reader.GetInt64(ords.SaturdayTicks); else sd.SaturdayTicks = 0L;

            MapBaseFields(reader, sd, ords);

            return sd;
        }));
    }
}
