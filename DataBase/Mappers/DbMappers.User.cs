using DataBase.Model.EntitiesServer;
using Microsoft.Data.Sqlite;

namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla User (dziedziczy BaseOrdinals)
    public class UserOrdinals : BaseOrdinals
    {
        public int Name { get; }
        public int Description { get; }
        public int Email { get; }
        public int PhoneNumber { get; }
        public int UserType { get; }
        public int IsEmailConfirm { get; }
        public int RememberMe { get; }

        public UserOrdinals(SqliteDataReader reader) : base(reader)
        {
            Name = reader.GetOrdinal(nameof(User.Name));
            Description = reader.GetOrdinal(nameof(User.Description));
            Email = reader.GetOrdinal(nameof(User.Email));
            PhoneNumber = reader.GetOrdinal(nameof(User.PhoneNumber));
            UserType = reader.GetOrdinal(nameof(User.UserType));
            IsEmailConfirm = reader.GetOrdinal(nameof(User.IsEmailConfirm));
            RememberMe = reader.GetOrdinal(nameof(User.RememberMe));
        }
    }

    private static void InitUserMappers()
    {
        _registry.Add(typeof(User), (Func<SqliteDataReader, object, User>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not UserOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for User mapper", nameof(ordinalsObj));

            var u = new User();

            if (!reader.IsDBNull(ords.Name)) u.Name = reader.GetString(ords.Name); else u.Name = string.Empty;
            if (!reader.IsDBNull(ords.Description)) u.Description = reader.GetString(ords.Description); else u.Description = string.Empty;
            if (!reader.IsDBNull(ords.Email)) u.Email = reader.GetString(ords.Email); else u.Email = string.Empty;
            if (!reader.IsDBNull(ords.PhoneNumber)) u.PhoneNumber = reader.GetString(ords.PhoneNumber); else u.PhoneNumber = string.Empty;
            if (!reader.IsDBNull(ords.UserType)) u.UserType = (UserType)reader.GetInt32(ords.UserType); else u.UserType = default;
            if (!reader.IsDBNull(ords.IsEmailConfirm)) u.IsEmailConfirm = reader.GetInt32(ords.IsEmailConfirm) == 1; else u.IsEmailConfirm = false;
            if (!reader.IsDBNull(ords.RememberMe)) u.RememberMe = reader.GetInt32(ords.RememberMe) == 1; else u.RememberMe = false;

            MapBaseFields(reader, u, ords);

            return u;
        }));
    }

    // Register RegisterUser mapper (inherits User) - includes Password if present in database
    public class RegisterUserOrdinals : UserOrdinals
    {
        public int Password { get; }
        public RegisterUserOrdinals(SqliteDataReader reader) : base(reader)
        {
            Password = reader.GetOrdinal(nameof(RegisterUser.Password));
        }
    }

    private static void InitRegisterUserMappers()
    {
        _registry.Add(typeof(RegisterUser), (Func<SqliteDataReader, object, RegisterUser>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not RegisterUserOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for RegisterUser mapper", nameof(ordinalsObj));

            var ru = new RegisterUser();

            // map base User fields via existing logic
            if (!reader.IsDBNull(ords.Name)) ru.Name = reader.GetString(ords.Name); else ru.Name = string.Empty;
            if (!reader.IsDBNull(ords.Description)) ru.Description = reader.GetString(ords.Description); else ru.Description = string.Empty;
            if (!reader.IsDBNull(ords.Email)) ru.Email = reader.GetString(ords.Email); else ru.Email = string.Empty;
            if (!reader.IsDBNull(ords.PhoneNumber)) ru.PhoneNumber = reader.GetString(ords.PhoneNumber); else ru.PhoneNumber = string.Empty;
            if (!reader.IsDBNull(ords.UserType)) ru.UserType = (UserType)reader.GetInt32(ords.UserType); else ru.UserType = default;
            if (!reader.IsDBNull(ords.IsEmailConfirm)) ru.IsEmailConfirm = reader.GetInt32(ords.IsEmailConfirm) == 1; else ru.IsEmailConfirm = false;
            if (!reader.IsDBNull(ords.RememberMe)) ru.RememberMe = reader.GetInt32(ords.RememberMe) == 1; else ru.RememberMe = false;

            if (!reader.IsDBNull(ords.Password)) ru.Password = reader.GetString(ords.Password); else ru.Password = string.Empty;

            MapBaseFields(reader, ru, ords);

            return ru;
        }));
    }

    // LoginUser is not BaseEntities, but we still provide a mapper if stored
    public class LoginUserOrdinals
    {
        public int Email { get; }
        public int Password { get; }
        public int RememberMe { get; }
        public LoginUserOrdinals(SqliteDataReader reader)
        {
            Email = reader.GetOrdinal(nameof(LoginUser.Email));
            Password = reader.GetOrdinal(nameof(LoginUser.Password));
            RememberMe = reader.GetOrdinal(nameof(LoginUser.RememberMe));
        }
    }

    private static void InitLoginUserMappers()
    {
        _registry.Add(typeof(LoginUser), (Func<SqliteDataReader, object, LoginUser>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not LoginUserOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for LoginUser mapper", nameof(ordinalsObj));

            var lu = new LoginUser();
            if (!reader.IsDBNull(ords.Email)) lu.Email = reader.GetString(ords.Email); else lu.Email = string.Empty;
            if (!reader.IsDBNull(ords.Password)) lu.Password = reader.GetString(ords.Password); else lu.Password = string.Empty;
            if (!reader.IsDBNull(ords.RememberMe)) lu.RememberMe = reader.GetInt32(ords.RememberMe) == 1; else lu.RememberMe = false;

            return lu;
        }));
    }

    // ConfirmCode (BaseEntities<int>)
    public class ConfirmCodeOrdinals : BaseOrdinals
    {
        public int UserId { get; }
        public int Code { get; }
        public int ExpireDate { get; }

        public ConfirmCodeOrdinals(SqliteDataReader reader) : base(reader)
        {
            UserId = reader.GetOrdinal(nameof(ConfirmCode.UserId));
            Code = reader.GetOrdinal(nameof(ConfirmCode.Code));
            ExpireDate = reader.GetOrdinal(nameof(ConfirmCode.ExpireDate));
        }
    }

    private static void InitConfirmCodeMappers()
    {
        _registry.Add(typeof(ConfirmCode), (Func<SqliteDataReader, object, ConfirmCode>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ConfirmCodeOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for ConfirmCode mapper", nameof(ordinalsObj));

            var cc = new ConfirmCode();

            if (!reader.IsDBNull(ords.UserId))
            {
                var s = reader.GetString(ords.UserId);
                cc.UserId = string.IsNullOrEmpty(s) ? Guid.Empty : new Guid(s);
            }
            else cc.UserId = Guid.Empty;

            if (!reader.IsDBNull(ords.Code)) cc.Code = reader.GetInt32(ords.Code); else cc.Code = 0;
            if (!reader.IsDBNull(ords.ExpireDate)) cc.ExpireDate = reader.GetInt64(ords.ExpireDate); else cc.ExpireDate = 0L;

            MapBaseFields(reader, cc, ords);

            return cc;
        }));
    }
}
