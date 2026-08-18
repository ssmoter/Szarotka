using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.Extensions.Logging.Abstractions;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface IResetPasswordService
    {
        Task ChangePassword(Guid id, string password);
        ConfirmCode? GetConfirmCode(int code, bool remove = false);
        Task<User> GetUserIdFromEmail(string email);
    }

    public class ResetPasswordService(IAccessDataBaseAoT db, IUserValidation userValidation, ITimeService time, ILogger<ResetPasswordService>? logger = null) : IResetPasswordService
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly ITimeService _time = time;
        private readonly ILogger<ResetPasswordService> _logger = logger ?? NullLogger<ResetPasswordService>.Instance;

        public async Task<User> GetUserIdFromEmail(string email)
        {
            _logger.LogInformation("GetUserIdFromEmail started for email={Email}", email);
            string sql = UserQuery.GetIdFromEmail(email);
            var Email = email;
            var ids = await _db.DbAsyncAoT.QueryAsync<User>(sql, new() { [nameof(Email)] = Email });
            var id = ids.FirstOrDefault();

            _userValidation.AccountNotFound(id);

            _userValidation.Validation.Throw();

            _logger.LogInformation("GetUserIdFromEmail succeeded for email={Email} userId={UserId}", email, id?.Id);
            return id!;
        }

        public ConfirmCode? GetConfirmCode(int code, bool remove = false)
        {
            _logger.LogInformation("GetConfirmCode started for code={Code}", code);

            ConfirmCode? codeResult;
            if (remove)
            {
                DictionaryList.ResetPasswordCodes.TryGetValue(code, out codeResult);
            }
            else
            {
                DictionaryList.ResetPasswordCodes.TryRemove(code, out codeResult);
            }
            _logger.LogInformation("GetConfirmCode returned {Found} result for code={Code}", codeResult is not null, code);
            return codeResult;
        }

        public async Task ChangePassword(Guid id, string password)
        {
            _logger.LogInformation("ChangePassword started for userId={UserId}", id);
            var Password = Hash.PasswordSHA256(password);
            var UpdateTicks = _time.UtcNow().Ticks;
            Guid Id = id;
            Guid UserUpdatedId = id;
            var slq = UserQuery.UpdatePassword(Password, UpdateTicks, UserUpdatedId, Id);
            try
            {
                await _db.DbAsyncAoT.ExecuteAsync(slq, new()
                {
                    [nameof(Password)] = Password,
                    [nameof(UpdateTicks)] = UpdateTicks,
                    [nameof(UserUpdatedId)] = UserUpdatedId,
                    [nameof(Id)] = Id
                });
                _logger.LogInformation("ChangePassword succeeded for userId={UserId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword failed for userId={UserId}", id);
                throw;
            }
        }

    }
}
