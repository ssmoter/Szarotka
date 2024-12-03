namespace Shared.Service
{
    public interface IUpdateDataBase
    {
        Task Update(int oldVersion, int newVersion, Action<double, int> updateAction);
    }
}
