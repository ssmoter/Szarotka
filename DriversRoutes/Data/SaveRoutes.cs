using Shared.Data;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Service;
using DataBase.Data;

namespace DriversRoutes.Data
{
    public class SaveRoutes(IAccessDataBase db) : ISaveRoutes
    {
        readonly IAccessDataBase _db = db;

        public async Task SaveCustomer(CustomerRoutes customer, byte[] idRoute)
        {
            byte[] customerId = Guid.NewGuid().ToByteArray();
            if (customer.Id == Guid.Empty)
            {
                customer.Id = new Guid(customerId);
                customer.RoutesId = new Guid(idRoute);
                customer.Created = DateTime.Now;
                customer.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer);
            }
            else
            {
                customer.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer);
            }

            if (customer.DayOfWeek.Id == Guid.Empty)
            {
                customer.DayOfWeek.Id = Guid.NewGuid();
                customer.DayOfWeek.CustomerId = new Guid(customerId);
                customer.DayOfWeek.Created = DateTime.Now;
                customer.DayOfWeek.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer.DayOfWeek);
            }
            else
            {
                customer.DayOfWeek.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer.DayOfWeek);
            }

            if (customer.ResidentialAddress.Id == Guid.Empty)
            {
                customer.ResidentialAddress.Id = Guid.NewGuid();
                customer.ResidentialAddress.CustomerId = new Guid(customerId);
                customer.ResidentialAddress.Created = DateTime.Now;
                customer.ResidentialAddress.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer.ResidentialAddress);
            }
            else
            {
                customer.ResidentialAddress.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer.ResidentialAddress);
            }
        }

        public async Task<bool> UpdateCustomersTime(IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, updateTime, selectedTime);
        }
        public async Task<bool> UpdateCustomersTime(List<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, updateTime, selectedTime);
        }
        public async Task<bool> UpdateCustomersTime(SelectedDayOfWeekRoutes[] selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, updateTime, selectedTime);
        }
        private async Task<bool> UpdateTime(IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes updateTime, SelectedDayOfWeekRoutes selectedTime)
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


            var task = _db.DataBaseAsync.UpdateAllAsync(dayOfs);

            await task;

            return task.IsCompletedSuccessfully;
        }
    }
}
