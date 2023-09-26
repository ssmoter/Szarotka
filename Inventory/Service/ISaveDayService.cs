using Inventory.Model.MVVM;

namespace Inventory.Service
{
    public interface ISaveDayService
    {
        Task<DayM> SaveDayMAsync(DayM dayM);
    }
}