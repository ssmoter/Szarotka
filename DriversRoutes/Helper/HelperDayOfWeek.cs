using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Helper
{
    public static class HelperDayOfWeek
    {
        public static void SetTodayDayOfWeek(this SelectedDayOfWeekRoutes dayOfWeek, DateTime dateTime)
        {
            var today = dateTime.DayOfWeek;
            switch (today)
            {
                case DayOfWeek.Sunday:
                    dayOfWeek.Sunday = true;
                    break;
                case DayOfWeek.Monday:
                    dayOfWeek.Monday = true;
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek.Tuesday = true;
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek.Wednesday = true;
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek.Thursday = true;
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek.Friday = true;
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek.Saturday = true;
                    break;
            }


        }

        public static bool IfAnyIsTrue(this SelectedDayOfWeekRoutes dayOf)
        {
            if (dayOf.Sunday)
            {
                return true;
            }
            if (dayOf.Monday)
            {
                return true;
            }
            if (dayOf.Tuesday)
            {
                return true;
            }
            if (dayOf.Wednesday)
            {
                return true;
            }
            if (dayOf.Thursday)
            {
                return true;
            }
            if (dayOf.Friday)
            {
                return true;
            }
            if (dayOf.Saturday)
            {
                return true;
            }
            return false;
        }

        public static DateTime GetTodayDatetimeFromSelectedDayOfWeekRoutes(this SelectedDayOfWeekRoutes dayOf)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;
            TimeSpan timespan;
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    timespan = dayOf.SundayTimeSpan;
                    now = new DateTime(2024, 9, 1, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Monday:
                    timespan = dayOf.MondayTimeSpan;
                    now = new DateTime(2024, 9, 2, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Tuesday:
                    timespan = dayOf.TuesdayTimeSpan;
                    now = new DateTime(2024, 9, 3, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Wednesday:
                    timespan = dayOf.WednesdayTimeSpan;
                    now = new DateTime(2024, 9, 4, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Thursday:
                    timespan = dayOf.ThursdayTimeSpan;
                    now = new DateTime(2024, 9, 5, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Friday:
                    timespan = dayOf.FridayTimeSpan;
                    now = new DateTime(2024, 9, 6, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                case DayOfWeek.Saturday:
                    timespan = dayOf.SaturdayTimeSpan;
                    now = new DateTime(2024, 9, 7, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds, timespan.Nanoseconds);
                    break;
                default:
                    break;
            }
            return now;
        }

    }


}
