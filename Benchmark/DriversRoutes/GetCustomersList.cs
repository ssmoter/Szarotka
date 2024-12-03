using BenchmarkDotNet.Attributes;

using DriversRoutes.Data;
using DriversRoutes.Service;

using DataBase.Model.EntitiesRoutes;
using DataBase.Data;

namespace Benchmark.DriversRoutes
{

    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    public class GetCustomersList
    {
        private readonly AccessDataBase _db;
        private readonly ISelectRoutes _selectRoutes;

        private readonly Routes _routes = new()
        {
            Id = new Guid("baf3bb5a-59f6-5524-10d6-2d4c3c84b98b")
        };
        private readonly SelectedDayOfWeekRoutes _week = new();
        public GetCustomersList()
        {
            _db = new();
            _selectRoutes = new SelectRoutes(_db);
        }
        [Benchmark]
        public CustomerRoutes[] GetCustomerRoutesQuery()
        {
            var result = _selectRoutes.GetCustomerRoutesQuery(_routes, _week);

            return result;
        }

        [Benchmark]
        public async Task<CustomerRoutes[]> GetCustomerRoutesQueryAsync()
        {
            var result = await _selectRoutes.GetCustomerRoutesQueryAsync(_routes, _week);

            return result;
        }
    }
}
