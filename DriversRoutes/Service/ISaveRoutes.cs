using DriversRoutes.Model;

namespace DriversRoutes.Service
{
    public interface ISaveRoutes
    {
        Task SaveCustomer(CustomerRoutes customer, byte[] idRoute);
    }
}