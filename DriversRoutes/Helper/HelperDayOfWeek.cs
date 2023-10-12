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
    }
}
