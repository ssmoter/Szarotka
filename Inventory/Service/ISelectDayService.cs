using DataBase.Model.EntitiesInventory;

namespace Inventory.Service
{
    public interface ISelectDayService
    {
        Task<Day> GetDayProcedure(DateTime createdDate);
        Task<Day> GetDayProcedure(Guid id);
        Task<(Driver[] drivers, Day[] days)> GetDaysAndDrivers(long from, long to, Guid[] selectedDriverName, bool moreData);
    }
}