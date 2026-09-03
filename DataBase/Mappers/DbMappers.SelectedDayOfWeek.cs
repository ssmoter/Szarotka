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
            CustomerId = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.CustomerId));

            Sunday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Sunday));
            SundayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.SundayTicks));

            Monday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Monday));
            MondayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.MondayTicks));

            Tuesday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Tuesday));
            TuesdayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.TuesdayTicks));

            Wednesday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Wednesday));
            WednesdayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.WednesdayTicks));

            Thursday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Thursday));
            ThursdayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.ThursdayTicks));

            Friday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Friday));
            FridayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.FridayTicks));

            Saturday = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.Saturday));
            SaturdayTicks = reader.GetOptionalOrdinal(nameof(SelectedDayOfWeekRoutes.SaturdayTicks));
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

            if (ords.CustomerId >= 0 && !reader.IsDBNull(ords.CustomerId))
            {
                var s = reader.GetString(ords.CustomerId);
                sd.CustomerId = string.IsNullOrEmpty(s) ? Guid.Empty : new Guid(s);
            }
            else sd.CustomerId = Guid.Empty;

            if (ords.Sunday >= 0 && !reader.IsDBNull(ords.Sunday)) sd.Sunday = reader.GetInt32(ords.Sunday) == 1; else sd.Sunday = false;
            if (ords.SundayTicks >= 0 && !reader.IsDBNull(ords.SundayTicks)) sd.SundayTicks = reader.GetInt64(ords.SundayTicks); else sd.SundayTicks = 0L;

            if (ords.Monday >= 0 && !reader.IsDBNull(ords.Monday)) sd.Monday = reader.GetInt32(ords.Monday) == 1; else sd.Monday = false;
            if (ords.MondayTicks >= 0 && !reader.IsDBNull(ords.MondayTicks)) sd.MondayTicks = reader.GetInt64(ords.MondayTicks); else sd.MondayTicks = 0L;

            if (ords.Tuesday >= 0 && !reader.IsDBNull(ords.Tuesday)) sd.Tuesday = reader.GetInt32(ords.Tuesday) == 1; else sd.Tuesday = false;
            if (ords.TuesdayTicks >= 0 && !reader.IsDBNull(ords.TuesdayTicks)) sd.TuesdayTicks = reader.GetInt64(ords.TuesdayTicks); else sd.TuesdayTicks = 0L;

            if (ords.Wednesday >= 0 && !reader.IsDBNull(ords.Wednesday)) sd.Wednesday = reader.GetInt32(ords.Wednesday) == 1; else sd.Wednesday = false;
            if (ords.WednesdayTicks >= 0 && !reader.IsDBNull(ords.WednesdayTicks)) sd.WednesdayTicks = reader.GetInt64(ords.WednesdayTicks); else sd.WednesdayTicks = 0L;

            if (ords.Thursday >= 0 && !reader.IsDBNull(ords.Thursday)) sd.Thursday = reader.GetInt32(ords.Thursday) == 1; else sd.Thursday = false;
            if (ords.ThursdayTicks >= 0 && !reader.IsDBNull(ords.ThursdayTicks)) sd.ThursdayTicks = reader.GetInt64(ords.ThursdayTicks); else sd.ThursdayTicks = 0L;

            if (ords.Friday >= 0 && !reader.IsDBNull(ords.Friday)) sd.Friday = reader.GetInt32(ords.Friday) == 1; else sd.Friday = false;
            if (ords.FridayTicks >= 0 && !reader.IsDBNull(ords.FridayTicks)) sd.FridayTicks = reader.GetInt64(ords.FridayTicks); else sd.FridayTicks = 0L;

            if (ords.Saturday >= 0 && !reader.IsDBNull(ords.Saturday)) sd.Saturday = reader.GetInt32(ords.Saturday) == 1; else sd.Saturday = false;
            if (ords.SaturdayTicks >= 0 && !reader.IsDBNull(ords.SaturdayTicks)) sd.SaturdayTicks = reader.GetInt64(ords.SaturdayTicks); else sd.SaturdayTicks = 0L;

            MapBaseFields(reader, sd, ords);

            return sd;
        }));
    }
}
