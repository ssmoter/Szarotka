using Inventory.Model.MVVM;

namespace Inventory.Service
{
    public interface ISelectDayService
    {
        Task GetCakeTable(DayM dayM);
        Task<DayM> GetDay();
        Task<DayM> GetDay(Guid id);
        Task<DayM> GetDay(string createdDate);
        Task GetProductTable(DayM dayM);
    }
}