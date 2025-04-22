using DataBase.Data;
using DataBase.Data.Get;

namespace Server.Requests
{
    public interface IDriverRoutesCustomerRoutesRequests
    {
        Task<IResult> GetCustomer(string id, CancellationToken token = default);
        Task<IResult> GetCustomers(string routeId, DayOfWeek[] selected_day, CancellationToken token);
    }

    public class DriverRoutesCustomerRoutesRequests : IDriverRoutesCustomerRoutesRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IGetDriverRoutesAoT _get;

        public DriverRoutesCustomerRoutesRequests(IAccessDataBase db, IGetDriverRoutesAoT get)
        {
            _get = get;
            _db = db;
        }

        public async Task<IResult> GetCustomer(string id, CancellationToken token = default)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    return Results.BadRequest("id is not a valid Guid");
                }

                token.ThrowIfCancellationRequested();

                var customer = await _get.CustomerRoute(idGuid);

                if (customer is not null)
                {
                    return Results.Ok(customer);
                }
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult> GetCustomers(string routeId, DayOfWeek[] selected_day, CancellationToken token = default)
        {
            try
            {
                if (!Guid.TryParse(routeId, out Guid idGuid))
                {
                    return Results.BadRequest("id is not a valid Guid");
                }

                token.ThrowIfCancellationRequested();

                selected_day ??= [];

                var customer = await _get.CustomerRoutes(idGuid, selected_day);

                if (customer is not null && customer.Count > 0)
                {
                    return Results.Ok(customer);
                }
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }
    }
}
