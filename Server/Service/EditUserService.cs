using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Service
{
    public interface IEditUserService
    {
        Task UpdateDescription(User user);
        Task UpdateEmail(User user);
        Task UpdateName(User user);
        Task UpdatePhoneNumber(User user);
        Task UpdateUserType(User user);
    }

    public class EditUserService(IAccessDataBaseAoT db, ITimeService timeService, ILogger<EditUserService>? logger = null) : IEditUserService
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly ITimeService _timeService = timeService;
        private readonly ILogger<EditUserService> _logger = logger ?? NullLogger<EditUserService>.Instance;

        public async Task UpdateName(User user)
        {
            _logger.LogInformation("UpdateName started for userId={UserId}", user?.Id);
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateName(user.Name, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(sql, new()
                {
                    [nameof(user.Name)] = user.Name,
                    [nameof(user.UpdatedTicks)] = user.UpdatedTicks,
                    [nameof(user.UserUpdatedId)] = user.UserUpdatedId,
                    [nameof(user.Id)] = user.Id
                });
                _logger.LogInformation("UpdateName succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateName failed for userId={UserId}", user.Id);
                throw;
            }
        }
        public async Task UpdateDescription(User user)
        {
            _logger.LogInformation("UpdateDescription started for userId={UserId}", user?.Id);
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateDescription(user.Description, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(sql, new()
                {
                    [nameof(user.Description)] = user.Description,
                    [nameof(user.UpdatedTicks)] = user.UpdatedTicks,
                    [nameof(user.UserUpdatedId)] = user.UserUpdatedId,
                    [nameof(user.Id)] = user.Id
                 });
                _logger.LogInformation("UpdateDescription succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDescription failed for userId={UserId}", user.Id);
                throw;
            }
        }
        public async Task UpdateEmail(User user)
        {
            _logger.LogInformation("UpdateEmail started for userId={UserId}", user?.Id);
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateEmail(user.Email, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(sql, new()
                {
                    [nameof(user.Email)] = user.Email,
                    [nameof(user.UpdatedTicks)] = user.UpdatedTicks,
                    [nameof(user.UserUpdatedId)] = user.UserUpdatedId,
                    [nameof(user.Id)] = user.Id
                });
                _logger.LogInformation("UpdateEmail succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateEmail failed for userId={UserId}", user.Id);
                throw;
            }
        }
        public async Task UpdatePhoneNumber(User user)
        {
            _logger.LogInformation("UpdatePhoneNumber started for userId={UserId}", user?.Id);
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdatePhoneNumber(user.PhoneNumber, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(sql, new()
                {
                    [nameof(user.PhoneNumber)] = user.PhoneNumber,
                    [nameof(user.UpdatedTicks)] = user.UpdatedTicks,
                    [nameof(user.UserUpdatedId)] = user.UserUpdatedId,
                    [nameof(user.Id)] = user.Id
                });
                _logger.LogInformation("UpdatePhoneNumber succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdatePhoneNumber failed for userId={UserId}", user.Id);
                throw;
            }
        }
        public async Task UpdateUserType(User user)
        {
            _logger.LogInformation("UpdateUserType started for userId={UserId}", user?.Id);
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateUserType(user.UserType, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(sql, new()
                {
                    [nameof(user.UserType)] = user.UserType,
                    [nameof(user.UpdatedTicks)] = user.UpdatedTicks,
                    [nameof(user.UserUpdatedId)] = user.UserUpdatedId,
                    [nameof(user.Id)] = user.Id
                });
                _logger.LogInformation("UpdateUserType succeeded for userId={UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUserType failed for userId={UserId}", user.Id);
                throw;
            }
        }

    }
}
