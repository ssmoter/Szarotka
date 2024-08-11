using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Service
{
    public interface ISaveRoutes
    {
        Task SaveCustomer(CustomerRoutes customer, byte[] idRoute);
        Task<bool> UpdateCustomersTime(IEnumerable<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime);
        Task<bool> UpdateCustomersTime(List<SelectedDayOfWeekRoutes> selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime);
        Task<bool> UpdateCustomersTime(SelectedDayOfWeekRoutes[] selectedDays, SelectedDayOfWeekRoutes upddateTime, SelectedDayOfWeekRoutes selectedTime);
    }
}