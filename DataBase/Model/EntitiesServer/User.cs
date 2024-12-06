using CommunityToolkit.Mvvm.ComponentModel;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesServer
{
    [JsonSerializable(typeof(User))]
    internal partial class UserJsonSerializerContext : JsonSerializerContext
    { }
    public partial class User : BaseEntities<Guid>
    {
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        private string description = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string phoneNumber = "";

        [ObservableProperty]
        private UserType userType;

        [ObservableProperty]
        private bool isDelete;

    }
    [JsonSerializable(typeof(RegisterUser))]
    public partial class RegisterUserJsonSerializerContext : JsonSerializerContext
    { }

    public partial class RegisterUser : User
    {
        [ObservableProperty]
        private string password = "";
    }
    [JsonSerializable(typeof(LoginUser))]
    internal partial class LoginUserJsonSerializerContext : JsonSerializerContext
    { }
    public partial class LoginUser : ObservableObject
    {
        [ObservableProperty]
        private string email = "";
        [ObservableProperty]
        private string password = "";
    }

    public enum UserType
    {
        Driver = 0,
        Confectioner = 1,
        Baker = 2,
    }

}
