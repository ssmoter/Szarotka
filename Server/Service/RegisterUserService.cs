using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Helper;
using Server.Model;
using Server.SqlQuery;

namespace Server.Service
{
    public interface IRegisterUserService
    {
        Task<User> GetUserEmailFromCodeAndRemoveOld(int code);
        Task InsertCodeEmailAndRemoveOld(RegisterConfirmEmailUser user);
        Task<RegisterUser> InsertNewUser(RegisterUser registerUser);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly AccessDataBase _db;
        public RegisterUserService(AccessDataBase db)
        {
            _db = db;
        }

        public async Task<RegisterUser> InsertNewUser(RegisterUser registerUser)
        {
            registerUser.IsDelete = false;
            registerUser.IsEmailConfirm = false;
            var time = DateTime.UtcNow;
            registerUser.Created = time;
            registerUser.Updated = time;
            registerUser.Id = Guid.NewGuid();
            registerUser.Password = Hash.PasswordSHA256(registerUser.Password);

            var query = RegisterUserQuery.RegisterNewUser(registerUser);
            var task = _db.DataBaseAsync.ExecuteAsync(query);

            await task;

            if (task.IsCompletedSuccessfully)
            {
                return registerUser;
            }

            throw new ArgumentException();
        }
        public async Task InsertCodeEmailAndRemoveOld(RegisterConfirmEmailUser user)
        {
            user.ExpireDate = user.Created.AddMinutes(10).Ticks;
            var codeOldTask = _db.DataBaseAsync.ExecuteAsync(RegisterUserQuery.RemoveExpireCode());
            var codeNewTask = _db.DataBaseAsync.ExecuteAsync(RegisterUserQuery.EmailConfirmInsert(user));

            await Task.WhenAll(codeOldTask, codeNewTask);
        }
        public async Task<User> GetUserEmailFromCodeAndRemoveOld(int code)
        {
            var userEmails = await _db.DataBaseAsync.QueryAsync<RegisterConfirmEmailUser>(RegisterUserQuery.EmailConfirmCheck(code));
            var userEmail = userEmails.FirstOrDefault();
            ValidationException validationException = new();

            if (userEmail is null)
            {
                validationException.AddError(new ValidationException.Valid("Not find a selected Code", EnumsList.Validation.EmailCodeNotExist));
                throw validationException;
            }
            if (userEmail.ExpireDate < DateTime.UtcNow.Ticks)
            {
                validationException.AddError(new ValidationException.Valid("The expiration time has passed", EnumsList.Validation.EmailCodeIsExpire));
                throw validationException;
            }

            var user = new User()
            {
                Id = userEmail.UserId,
                IsEmailConfirm = true,
                Updated = DateTime.UtcNow,
            };

            var userTask = _db.DataBaseAsync.ExecuteAsync(RegisterUserQuery.EmailIsConfirmUpdate(user));
            var codeTask = _db.DataBaseAsync.ExecuteAsync(RegisterUserQuery.RemoveExpireCode());
            var emailTask = _db.DataBaseAsync.QueryAsync<User>(RegisterUserQuery.GetEmailFromId(userEmail.UserId));
            try
            {
                await Task.WhenAll(userTask, codeTask, emailTask);
            }
            catch (Exception ex)
            {
                throw;
            }

            var email = emailTask.Result.FirstOrDefault();

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return email;
        }




    }
}
