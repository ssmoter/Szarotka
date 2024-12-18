using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Service
{
    public interface ISelectRoutes
    {
        //Task<CustomerRoutes[]> GetCustomerRoutes(Routes routes, SelectedDayOfWeekRoutes dayOf);
        CustomerRoutes[] GetCustomerRoutesQuery(Routes routes, SelectedDayOfWeekRoutes week);
        Task<CustomerRoutes[]> GetCustomerRoutesQueryAsync(Routes routes, SelectedDayOfWeekRoutes dayOf);
        IAsyncEnumerable<CustomerRoutes> Test(Routes routes, SelectedDayOfWeekRoutes dayOf);
    }
}