using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesServer
{
    public partial class User : BaseEntities<Guid>
    {
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private UserType userType;

        [ObservableProperty]
        private bool isDelete;

    }

    public partial class RegisterUser : User
    {
        [ObservableProperty]
        private string password;
    }


    public partial class LoginUser : ObservableObject
    {
        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string password;
    }

    public enum UserType
    {
        Driver = 0,
        Confectioner = 1,
        Baker = 2,
    }

}
