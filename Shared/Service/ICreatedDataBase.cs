using DataBase.Model;

namespace Shared.Service
{
    public interface ICreatedDataBase
    {
        Task CreateBackUp();
        DataBaseVersion GetCurrentVersion();
        Task<bool> UpdateDataBase(Action<double, int> updateDataBase, Action<double, int> updateInventory, Action<double, int> updateDriverRoutes);
    }
}