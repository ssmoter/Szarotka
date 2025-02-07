using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

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

    public class EditUserService : IEditUserService
    {
        private readonly IAccessDataBase _db;
        private readonly ITimeService _timeService;

        public EditUserService(IAccessDataBase db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }



        public async Task UpdateName(User user)
        {
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateName(user);
            try
            {
                await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateDescription(User user)
        {
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateDescription(user);
            try
            {
                await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateEmail(User user)
        {
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateEmail(user);
            try
            {
                await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdatePhoneNumber(User user)
        {
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdatePhoneNumber(user);
            try
            {
                await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateUserType(User user)
        {
            user.Updated = _timeService.UtcNow();

            var sql = SqlQuery.UserQuery.UpdateUserType(user);
            try
            {
                await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
