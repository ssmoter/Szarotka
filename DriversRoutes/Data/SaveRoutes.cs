using DataBase.Data;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SaveRoutes(AccessDataBase db) : ISaveRoutes
    {
        readonly DataBase.Data.AccessDataBase _db = db;

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

        public async Task<bool> UpdateCustomersTime(IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, upddateTime, selectedTime);
        }
        public async Task<bool> UpdateCustomersTime(List<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, upddateTime, selectedTime);
        }
        public async Task<bool> UpdateCustomersTime(SelectedDayOfWeekRoutes[] selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            return await UpdateTime(selectedDays, upddateTime, selectedTime);
        }
        private async Task<bool> UpdateTime(IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime)
        {
            IEnumerable<SelectedDayOfWeekRoutes> dayOfs = [];
            var timeZero = new TimeSpan(0);

            if (upddateTime.Sunday)
            {
                if (upddateTime.SundayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Sunday && x.SundayTimeSpan > selectedTime.SundayTimeSpan);
                }
                else if (upddateTime.SundayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Sunday && x.SundayTimeSpan < selectedTime.SundayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.SundayTimeSpan += upddateTime.SundayTimeSpan;
                }
            }
            if (upddateTime.Monday)
            {
                if (upddateTime.MondayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Monday && x.MondayTimeSpan > selectedTime.MondayTimeSpan);
                }
                else if (upddateTime.MondayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Monday && x.MondayTimeSpan < selectedTime.MondayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.MondayTimeSpan += upddateTime.MondayTimeSpan;
                }
            }
            if (upddateTime.Tuesday)
            {
                if (upddateTime.TuesdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Tuesday && x.TuesdayTimeSpan > selectedTime.TuesdayTimeSpan);
                }
                else if (upddateTime.TuesdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Tuesday && x.TuesdayTimeSpan < selectedTime.TuesdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.TuesdayTimeSpan += upddateTime.TuesdayTimeSpan;
                }
            }
            if (upddateTime.Wednesday)
            {
                if (upddateTime.WednesdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Wednesday && x.WednesdayTimeSpan > selectedTime.WednesdayTimeSpan);
                }
                else if (upddateTime.WednesdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Wednesday && x.WednesdayTimeSpan < selectedTime.WednesdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.WednesdayTimeSpan += upddateTime.WednesdayTimeSpan;
                }
            }
            if (upddateTime.Thursday)
            {
                if (upddateTime.ThursdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Thursday && x.ThursdayTimeSpan > selectedTime.ThursdayTimeSpan);
                }
                else if (upddateTime.ThursdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Thursday && x.ThursdayTimeSpan < selectedTime.ThursdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.ThursdayTimeSpan += upddateTime.ThursdayTimeSpan;
                }
            }
            if (upddateTime.Friday)
            {
                if (upddateTime.FridayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Friday && x.FridayTimeSpan > selectedTime.FridayTimeSpan);
                }
                else if (upddateTime.FridayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Friday && x.FridayTimeSpan < selectedTime.FridayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.FridayTimeSpan += upddateTime.FridayTimeSpan;
                }
            }
            if (upddateTime.Saturday)
            {
                if (upddateTime.SaturdayTimeSpan > timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Saturday && x.SaturdayTimeSpan > selectedTime.SaturdayTimeSpan);
                }
                else if (upddateTime.SaturdayTimeSpan < timeZero)
                {
                    dayOfs = selectedDays.Where(x => x.Saturday && x.SaturdayTimeSpan < selectedTime.SaturdayTimeSpan);
                }
                foreach (var item in dayOfs)
                {
                    item.SaturdayTimeSpan += upddateTime.SaturdayTimeSpan;
                }
            }


            var task = _db.DataBaseAsync.UpdateAllAsync(dayOfs);

            await task;

            return task.IsCompletedSuccessfully;
        }
    }
}
