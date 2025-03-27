using DataBase.Model.EntitiesServer;


namespace Server.SqlQuery
{
    public static class UserQuery
    {
        public static string RegisterNewUser(Guid Id,
            long CreatedTicks,
            long UpdatedTicks,
            string Name,
            string Description,
            string Email,
            string PhoneNumber,
            UserType UserType,
            bool IsDelete,
            bool IsEmailConfirm,
            string Password)
        {
            string sql = $@"
INSERT INTO {nameof(User)}
(
{nameof(User.Id)},
{nameof(User.CreatedTicks)},
{nameof(User.UpdatedTicks)},
{nameof(User.Name)},
{nameof(User.Description)},
{nameof(User.Email)},
{nameof(User.PhoneNumber)},
{nameof(User.UserType)},
{nameof(User.IsDelete)},
{nameof(User.IsEmailConfirm)},
{nameof(RegisterUser.Password)},
{nameof(User.UserUpdatedId)},
{nameof(User.UserCreatedId)}
)
VALUES(
@{nameof(Id)},
@{nameof(CreatedTicks)},
@{nameof(UpdatedTicks)},
@{nameof(Name)},
@{nameof(Description)},
@{nameof(Email)},
@{nameof(PhoneNumber)},
@{nameof(UserType)},
@{nameof(IsDelete)},
@{nameof(IsEmailConfirm)},
@{nameof(Password)},
@{nameof(Id)},
@{nameof(Id)}
)
";
            return sql;
        }
        public static string ConfirmCodeInsert(long CreatedTicks, long UpdatedTicks, Guid UserId, int Code, long ExpireDate)
        {
            string sql = $@"
INSERT INTO {nameof(ConfirmCode)}
(
{nameof(ConfirmCode.CreatedTicks)},
{nameof(ConfirmCode.UpdatedTicks)},
{nameof(ConfirmCode.UserId)},
{nameof(ConfirmCode.Code)},
{nameof(ConfirmCode.ExpireDate)}
)
VALUES(
@{nameof(CreatedTicks)},
@{nameof(UpdatedTicks)},
@{nameof(UserId)},
@{nameof(Code)},
@{nameof(ExpireDate)}
)
";
            return sql;
        }
        public static string CodeConfirmCheck(int Code)
        {
            string sql = $@"
SELECT
{nameof(ConfirmCode.UserId)},
{nameof(ConfirmCode.ExpireDate)}
FROM {nameof(ConfirmCode)}
WHERE
{nameof(ConfirmCode.Code)} = @{nameof(Code)}
LIMIT 1
";
            return sql;
        }
        public static string EmailIsConfirmUpdate(bool IsEmailConfirm, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.IsEmailConfirm)} = @{nameof(IsEmailConfirm)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string RemoveExpireCode(long TicksNow)
        {
            string sql = $@"
DELETE
FROM {nameof(ConfirmCode)}
WHERE
{nameof(ConfirmCode.ExpireDate)} < @{nameof(TicksNow)}
";
            return sql;
        }
        public static string GetEmailFromId(Guid Id)
        {
            string sql = $@"
SELECT
{nameof(User.Email)}
FROM {nameof(User)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string GetIdFromEmail(string Email)
        {
            string sql = $@"
SELECT
{nameof(User.Id)},
{nameof(User.Email)}
FROM {nameof(User)}
WHERE
{nameof(User.Email)} = @{nameof(Email)}
";
            return sql;
        }






        public static string UpdateName(string Name, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Name)} = @{nameof(Name)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string UpdateDescription(string Description, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Description)} = @{nameof(Description)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string UpdateEmail(string Email, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.Email)} = @{nameof(Email)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string UpdatePhoneNumber(string PhoneNumber, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.PhoneNumber)} = @{nameof(PhoneNumber)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string UpdateUserType(UserType UserType, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(User.UserType)} = @{nameof(UserType)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }
        public static string UpdatePassword(string Password, long UpdatedTicks, Guid UserUpdatedId, Guid Id)
        {
            string sql = $@"
UPDATE {nameof(User)}
SET
{nameof(RegisterUser.Password)} = @{nameof(Password)},
{nameof(User.UpdatedTicks)} = @{nameof(UpdatedTicks)},
{nameof(User.UserUpdatedId)} = @{nameof(UserUpdatedId)}
WHERE
{nameof(User.Id)} = @{nameof(Id)}
";
            return sql;
        }

    }
}
