using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

namespace Server.Requests
{
    public interface IDriverRoutesCustomerRoutesRequests
    {
        Task<IResult> GetCustomer(string id, CancellationToken token = default);
        Task<IResult> GetCustomers(string routeId, DayOfWeek[] selected_day, bool isDelete = false, CancellationToken token = default);
        Task<IResult> GetCustomers(string[] ids, CancellationToken token = default);
        Task<IResult> UpdateCustomer(CustomerRoutes customer, bool forceUpdate, CancellationToken token = default);
        Task<IResult> UpdateCustomers(IList<CustomerRoutes> customers, bool forceUpdate, CancellationToken token = default);
    }

    public class DriverRoutesCustomerRoutesRequests : IDriverRoutesCustomerRoutesRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IGetDriverRoutesAoT _get;
        private readonly ISaveDriverRoutesAoT _save;
        private readonly IUpdateLogService _updateLogService;

        public DriverRoutesCustomerRoutesRequests(IAccessDataBase db, IGetDriverRoutesAoT get, ISaveDriverRoutesAoT save, IUpdateLogService updateLogService)
        {
            _get = get;
            _db = db;
            _save = save;
            _updateLogService = updateLogService;
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
        public async Task<IResult> GetCustomers(string routeId, DayOfWeek[] selected_day, bool isDelete = false, CancellationToken token = default)
        {
            try
            {
                if (!Guid.TryParse(routeId, out Guid idGuid))
                {
                    return Results.BadRequest("id is not a valid Guid");
                }

                token.ThrowIfCancellationRequested();

                selected_day ??= [];

                var customer = await _get.CustomerRoutes(idGuid, selected_day, isDelete);

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

        public async Task<IResult> GetCustomers(string[] ids, CancellationToken token = default)
        {
            try
            {
                CustomerRoutes[] customers = new CustomerRoutes[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    string? id = ids[i];
                    token.ThrowIfCancellationRequested();

                    if (!Guid.TryParse(id, out Guid guidId))
                    {
                        continue;
                    }

                    var customer = await _get.CustomerRoute(guidId);
                    if (customer is null)
                    {
                        continue;
                    }
                    customers[i] = customer;
                }
                return Results.Ok(customers.Where(x => x is not null).ToList());
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


        public async Task<IResult> UpdateCustomer(CustomerRoutes customer, bool forceUpdate, CancellationToken token = default)
        {
            try
            {
                if (customer.Id == Guid.Empty || customer.Longitude < 0 || customer.Latitude < 0)
                {
                    throw new ArgumentNullException(nameof(customer));
                }

                token.ThrowIfCancellationRequested();

                (bool canUpdate, CustomerRoutes? isExist) = await RoutesDifferences.Check(_get, customer, forceUpdate);

                if (canUpdate)
                {
                    await _save.SaveCustomerRoutes(customer, customer.UserUpdatedId.ToByteArray(), true);
                    await _save.SaveResidentialAddress(customer.ResidentialAddress, customer.UserUpdatedId.ToByteArray(), true);
                    await _save.SaveSelectedDayOfWeekRoutes(customer.DayOfWeek, customer.UserUpdatedId.ToByteArray(), true);

                    var updateLog = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                    {
                        IsServer = true,
                    }, customer);

                    return Results.Created(updateLog.Id.ToString(), updateLog);
                }

                return Results.Conflict(isExist);
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
        public async Task<IResult> UpdateCustomers(IList<CustomerRoutes> customers, bool forceUpdate, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                UpdateDifferences exists = new()
                {
                    UpdateDifferencesDriverRoutes = []
                };
                UpdateLog? firstLog = null;
                foreach (CustomerRoutes customer in customers)
                {
                    (bool canUpdate, CustomerRoutes? isExist) = await RoutesDifferences.Check(_get, customer, forceUpdate);

                    if (canUpdate)
                    {
                        await _save.SaveCustomerRoutes(customer, customer.UserUpdatedId.ToByteArray(), true);
                        await _save.SaveResidentialAddress(customer.ResidentialAddress, customer.UserUpdatedId.ToByteArray(), true);
                        await _save.SaveSelectedDayOfWeekRoutes(customer.DayOfWeek, customer.UserUpdatedId.ToByteArray(), true);

                        var log = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                        {
                            IsServer = true,
                        }, customer);

                        firstLog ??= log;
                    }
                    if (!canUpdate)
                    {
                        exists.UpdateDifferencesDriverRoutes.Add(new()
                        {
                            Update = customer,
                            Server = isExist!
                        });
                    }
                }
                if (exists.UpdateDifferencesDriverRoutes.Count == 0)
                {
                    return Results.Created(firstLog?.Id.ToString(), firstLog);
                }
                return Results.Conflict(exists);
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
