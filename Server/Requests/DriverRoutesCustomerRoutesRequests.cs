using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

    public class DriverRoutesCustomerRoutesRequests(IAccessDataBaseAoT db, IGetDriverRoutesAoT get, ISaveDriverRoutesAoT save, IUpdateLogService updateLogService, ILogger<DriverRoutesCustomerRoutesRequests>? logger = null) : IDriverRoutesCustomerRoutesRequests
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly IGetDriverRoutesAoT _get = get;
        private readonly ISaveDriverRoutesAoT _save = save;
        private readonly IUpdateLogService _updateLogService = updateLogService;
        private readonly ILogger<DriverRoutesCustomerRoutesRequests> _logger = logger ?? NullLogger<DriverRoutesCustomerRoutesRequests>.Instance;

        public async Task<IResult> GetCustomer(string id, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetCustomer started for id={Id}", id);
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    _logger.LogWarning("GetCustomer: id is not a valid Guid: {Id}", id);
                    return Results.BadRequest("id is not a valid Guid");
                }

                token.ThrowIfCancellationRequested();

                var customer = await _get.CustomerRoute(idGuid);

                if (customer is not null)
                {
                    _logger.LogInformation("GetCustomer: found customer for id={Id}", id);
                    return Results.Ok(customer);
                }
                _logger.LogInformation("GetCustomer: customer not found for id={Id}", id);
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetCustomer for id={Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCustomer for id={Id}", id);
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> GetCustomers(string routeId, DayOfWeek[] selected_day, bool isDelete = false, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetCustomers(routeId) started for routeId={RouteId}", routeId);
                if (!Guid.TryParse(routeId, out Guid idGuid))
                {
                    _logger.LogWarning("GetCustomers(routeId): id is not a valid Guid: {RouteId}", routeId);
                    return Results.BadRequest("id is not a valid Guid");
                }

                token.ThrowIfCancellationRequested();

                selected_day ??= [];
                _logger.LogDebug("GetCustomers(routeId): selected_day was null, assigned empty array for routeId={RouteId}", routeId);

                var customer = await _get.CustomerRoutes(idGuid, selected_day, isDelete);

                if (customer is not null && customer.Count > 0)
                {
                    _logger.LogInformation("GetCustomers(routeId): found {Count} customers for routeId={RouteId}", customer.Count, routeId);
                    return Results.Ok(customer);
                }
                _logger.LogInformation("GetCustomers(routeId): no customers found for routeId={RouteId}", routeId);
                return Results.NotFound();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetCustomers(routeId) for routeId={RouteId}", routeId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCustomers(routeId) for routeId={RouteId}", routeId);
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult> GetCustomers(string[] ids, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetCustomers(ids) started for {Count} ids", ids?.Length ?? 0);
                CustomerRoutes[] customers = new CustomerRoutes[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    string? id = ids[i];
                    token.ThrowIfCancellationRequested();

                    if (!Guid.TryParse(id, out Guid guidId))
                    {
                        _logger.LogWarning("GetCustomers(ids): skipping invalid Guid at index {Index}: {Id}", i, id);
                        continue;
                    }

                    var customer = await _get.CustomerRoute(guidId);
                    if (customer is null)
                    {
                        _logger.LogDebug("GetCustomers(ids): no customer found for id at index {Index}: {Id}", i, id);
                        continue;
                    }
                    customers[i] = customer;
                }
                var result = customers.Where(x => x is not null).ToList();
                _logger.LogInformation("GetCustomers(ids): returning {Count} customers", result.Count);
                return Results.Ok(result);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetCustomers(ids)");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCustomers(ids)");
                _db.SaveLog(ex);
                throw;
            }
        }


        public async Task<IResult> UpdateCustomer(CustomerRoutes customer, bool forceUpdate, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateCustomer started for customerId={CustomerId}", customer?.Id);
                if (customer.Id == Guid.Empty || customer.Longitude < 0 || customer.Latitude < 0)
                {
                    _logger.LogWarning("UpdateCustomer: invalid customer data for customerId={CustomerId}", customer?.Id);
                    throw new ArgumentNullException(nameof(customer));
                }

                token.ThrowIfCancellationRequested();

                (bool canUpdate, CustomerRoutes? isExist) = await ModelsDifferences.Check(_get, customer, forceUpdate);

                if (canUpdate)
                {
                    _logger.LogInformation("UpdateCustomer: saving customerId={CustomerId}", customer.Id);
                    await _save.SaveCustomerRoutes(customer, customer.UserUpdatedId.ToByteArray(), true);
                    await _save.SaveResidentialAddress(customer.ResidentialAddress, customer.UserUpdatedId.ToByteArray(), true);
                    await _save.SaveSelectedDayOfWeekRoutes(customer.DayOfWeek, customer.UserUpdatedId.ToByteArray(), true);

                    var updateLog = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                    {
                        IsServer = true,
                    }, customer);

                    _logger.LogInformation("UpdateCustomer: saved and created updateLogId={UpdateLogId} for customerId={CustomerId}", updateLog.Id, customer.Id);
                    return Results.Created(updateLog.Id.ToString(), updateLog);
                }
                _logger.LogInformation("UpdateCustomer: conflict for customerId={CustomerId}", customer.Id);
                return Results.Conflict(isExist);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateCustomer for customerId={CustomerId}", customer?.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateCustomer for customerId={CustomerId}", customer?.Id);
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> UpdateCustomers(IList<CustomerRoutes> customers, bool forceUpdate, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateCustomers started for {Count} customers", customers?.Count ?? 0);
                token.ThrowIfCancellationRequested();
                IList<UpdateDifference> exists = [];

                UpdateLog? firstLog = null;
                foreach (CustomerRoutes customer in customers)
                {
                    _logger.LogDebug("UpdateCustomers: processing customerId={CustomerId}", customer?.Id);
                    (bool canUpdate, CustomerRoutes? isExist) = await ModelsDifferences.Check(_get, customer, forceUpdate);

                    if (canUpdate)
                    {
                        _logger.LogInformation("UpdateCustomers: saving customerId={CustomerId}", customer.Id);
                        await _save.SaveCustomerRoutes(customer, customer.UserUpdatedId.ToByteArray(), true);
                        await _save.SaveResidentialAddress(customer.ResidentialAddress, customer.UserUpdatedId.ToByteArray(), true);
                        await _save.SaveSelectedDayOfWeekRoutes(customer.DayOfWeek, customer.UserUpdatedId.ToByteArray(), true);

                        var log = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                        {
                            IsServer = true,
                        }, customer);

                        firstLog ??= log;
                        _logger.LogInformation("UpdateCustomers: saved customerId={CustomerId} with updateLogId={UpdateLogId}", customer.Id, log.Id);
                    }
                    if (!canUpdate)
                    {
                        exists.Add(new UpdateDifference()
                        {
                            Update = customer,
                            Server = isExist!
                        });
                        _logger.LogInformation("UpdateCustomers: conflict for customerId={CustomerId}", customer.Id);
                    }
                }
                if (exists.Count == 0)
                {
                    _logger.LogInformation("UpdateCustomers: all customers updated, created updateLogId={UpdateLogId}", firstLog?.Id);
                    return Results.Created(firstLog?.Id.ToString(), firstLog);
                }
                _logger.LogInformation("UpdateCustomers: {ConflictCount} conflicts found", exists.Count);
                return Results.Conflict(exists);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateCustomers");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateCustomers");
                _db.SaveLog(ex);
                throw;
            }
        }
    }
}
