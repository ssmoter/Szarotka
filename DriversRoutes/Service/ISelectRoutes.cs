using DriversRoutes.Model;

namespace DriversRoutes.Service
{
    public interface ISelectRoutes
    {
        Task<CustomerRoutes[]> GetCustomerRoutes(Routes routes, SelectedDayOfWeekRoutes dayOf);
        Task<CustomerRoutes[]> GetCustomerRoutesQuery(Routes routes, SelectedDayOfWeekRoutes dayOf);
    }
}