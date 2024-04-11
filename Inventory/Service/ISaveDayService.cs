using DataBase.Model.EntitiesInventory;

namespace Inventory.Service
{
    public interface ISaveDayService
    {
        Task<Day> SaveDayAsync(Day value);
    }
}