using CommunityToolkit.Mvvm.ComponentModel;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesServer
{
    [JsonSerializable(typeof(User))]
    [JsonSourceGenerationOptions(WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    public partial class UserJsonSerializerContext : JsonSerializerContext
    { }
    public partial class User : BaseEntities<Guid>
    {
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value))
                {
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                if (SetProperty(ref description, value))
                {
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                if (SetProperty(ref email, value))
                {
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        private string phoneNumber = "";
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (SetProperty(ref phoneNumber, value))
                {
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        private UserType userType;
        public UserType UserType
        {
            get => userType;
            set
            {
                if (SetProperty(ref userType, value))
                {
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }
        public bool IsDelete { get; set; }

        private bool isEmailConfirm;
        public bool IsEmailConfirm
        {
            get => isEmailConfirm;
            set
            {
                if (SetProperty(ref isEmailConfirm, value))
                {
                    OnPropertyChanged(nameof(IsEmailConfirm));
                }
            }
        }

        [SQLite.Ignore]
        public string Token { get; set; } = "";
    }
    [JsonSerializable(typeof(RegisterUser))]
    [JsonSourceGenerationOptions(WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    public partial class RegisterUserJsonSerializerContext : JsonSerializerContext
    { }

    public partial class RegisterUser : User
    {
        private string password = "";
        public string Password
        {
            get => password;
            set
            {
                if (SetProperty(ref password, value))
                {
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
    }
    [JsonSerializable(typeof(LoginUser))]
    public partial class LoginUserJsonSerializerContext : JsonSerializerContext
    { }
    public partial class LoginUser : ObservableObject
    {
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                if (SetProperty(ref email, value))
                {
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        private string password = "";
        public string Password
        {
            get => password;
            set
            {
                if (SetProperty(ref password, value))
                {
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
    }

    public class RegisterConfirmEmailUser : BaseEntities<int>
    {
        public Guid UserId { get; set; }
        public int Code { get; set; }
        public long ExpireDate { get; set; }
        public RegisterConfirmEmailUser(Guid userId, int code)
        {
            UserId = userId;
            Code = code;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
        public RegisterConfirmEmailUser()
        {
            
        }
    }

    public enum UserType
    {
        Driver = 0,
        Confectioner = 1,
        Baker = 2,
    }

}
