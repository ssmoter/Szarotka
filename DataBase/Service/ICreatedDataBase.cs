using DataBase.Model;

namespace DataBase.Service
{
    public interface ICreatedDataBase
    {
        DataBaseVersion GetCurrentVersion();
        Task<bool> UpdateDataBase(Action<double, int> uppdateDataBase, Action<double, int> uppdateInventory, Action<double, int> uppdateDriverRoutes);
    }
}