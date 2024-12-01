using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Mvc;

namespace Server.Service
{
    public interface IRegisterUserService
    {
        Task<IActionResult> InsertNewUser(RegisterUser registerUser);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly DataBase.Data.AccessDataBase _db;
        public RegisterUserService(AccessDataBase db)
        {
            _db = db;
        }

        public async Task<IActionResult> InsertNewUser(RegisterUser registerUser)
        {


            return null;
        }

    }
}
