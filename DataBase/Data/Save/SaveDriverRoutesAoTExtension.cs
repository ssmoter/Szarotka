using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.Save
{
    public static class SaveDriverRoutesAoTExtension
    {
        public async static Task<bool> UpdateCustomersTime(this ISaveDriverRoutesAoT save, IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(save, selectedDays, updateTime, selectedTime);
        }
        public async static Task<bool> UpdateCustomersTime(this ISaveDriverRoutesAoT save, List<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(save, selectedDays, updateTime, selectedTime);
        }
        public async static Task<bool> UpdateCustomersTime(this ISaveDriverRoutesAoT save, SelectedDayOfWeekRoutes[] selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(save, selectedDays, updateTime, selectedTime);
        }
        private async static Task<bool> UpdateTime(this ISaveDriverRoutesAoT save, IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            IEnumerable<SelectedDayOfWeekRoutes> dayOfs = [];
            var timeZero = new TimeSpan(0);

            if (updateTime.Sunday)
            {
                if (updateTime.SundayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Sunday && x.SundayTimeSpan > selectedTime.SundayTimeSpan);
                }
                else if (updateTime.SundayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Sunday && x.SundayTimeSpan < selectedTime.SundayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.SundayTimeSpan += updateTime.SundayTimeSpan;
                }
            }
            if (updateTime.Monday)
            {
                if (updateTime.MondayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Monday && x.MondayTimeSpan > selectedTime.MondayTimeSpan);
                }
                else if (updateTime.MondayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Monday && x.MondayTimeSpan < selectedTime.MondayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.MondayTimeSpan += updateTime.MondayTimeSpan;
                }
            }
            if (updateTime.Tuesday)
            {
                if (updateTime.TuesdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Tuesday && x.TuesdayTimeSpan > selectedTime.TuesdayTimeSpan);
                }
                else if (updateTime.TuesdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Tuesday && x.TuesdayTimeSpan < selectedTime.TuesdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.TuesdayTimeSpan += updateTime.TuesdayTimeSpan;
                }
            }
            if (updateTime.Wednesday)
            {
                if (updateTime.WednesdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Wednesday && x.WednesdayTimeSpan > selectedTime.WednesdayTimeSpan);
                }
                else if (updateTime.WednesdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Wednesday && x.WednesdayTimeSpan < selectedTime.WednesdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.WednesdayTimeSpan += updateTime.WednesdayTimeSpan;
                }
            }
            if (updateTime.Thursday)
            {
                if (updateTime.ThursdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Thursday && x.ThursdayTimeSpan > selectedTime.ThursdayTimeSpan);
                }
                else if (updateTime.ThursdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Thursday && x.ThursdayTimeSpan < selectedTime.ThursdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.ThursdayTimeSpan += updateTime.ThursdayTimeSpan;
                }
            }
            if (updateTime.Friday)
            {
                if (updateTime.FridayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Friday && x.FridayTimeSpan > selectedTime.FridayTimeSpan);
                }
                else if (updateTime.FridayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Friday && x.FridayTimeSpan < selectedTime.FridayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.FridayTimeSpan += updateTime.FridayTimeSpan;
                }
            }
            if (updateTime.Saturday)
            {
                if (updateTime.SaturdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Saturday && x.SaturdayTimeSpan > selectedTime.SaturdayTimeSpan);
                }
                else if (updateTime.SaturdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Saturday && x.SaturdayTimeSpan < selectedTime.SaturdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.SaturdayTimeSpan += updateTime.SaturdayTimeSpan;
                }
            }

            var task = new Task[dayOfs.Count()];
            int taskCount = 0;
            foreach (var item in dayOfs)
            {
                task[taskCount] = save.SaveSelectedDayOfWeekRoutes(item, item.UserUpdatedId.ToByteArray());
                taskCount++;
            }
            await Task.WhenAll(task);
            return true;
        }




    }
}
