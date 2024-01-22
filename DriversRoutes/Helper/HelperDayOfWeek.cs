using DriversRoutes.Model;

namespace DriversRoutes.Helper
{
    public static class HelperDayOfWeek
    {
        public static void SetTodayDayOfWeek(this SelectedDayOfWeekRoutes dayOfWeek, DateTime dateTime)
        {
            var today = DateTime.Today.DayOfWeek;
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


    }
}
