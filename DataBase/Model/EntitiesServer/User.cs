using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesServer;


public partial class User : BaseEntities<Guid>
{
    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name)))
            {
                //OnPropertyChanged(nameof(Name));
            }
        }
    }
    private string description = "";
    public string Description
    {
        get => description;
        set
        {
            if (SetProperty(ref description, value, nameof(Description)))
            {
                //OnPropertyChanged(nameof(Description));
            }
        }
    }

    private string email = "";
    public string Email
    {
        get => email;
        set
        {
            if (SetProperty(ref email, value, nameof(Email)))
            {
                //OnPropertyChanged(nameof(Email));
            }
        }
    }
    private string phoneNumber = "";
    public string PhoneNumber
    {
        get => phoneNumber;
        set
        {
            if (SetProperty(ref phoneNumber, value, nameof(PhoneNumber)))
            {
                //OnPropertyChanged(nameof(PhoneNumber));
            }
        }
    }
    private UserType userType;
    public UserType UserType
    {
        get => userType;
        set
        {
            if (SetProperty(ref userType, value, nameof(UserType)))
            {
                //OnPropertyChanged(nameof(UserType));
            }
        }
    }
    private bool isDelete;
    public bool IsDelete
    {
        get => isDelete;
        set
        {
            if (!SetProperty(ref isDelete, value, nameof(IsDelete))) { }
        }
    }

    private bool isEmailConfirm;
    public bool IsEmailConfirm
    {
        get => isEmailConfirm;
        set
        {
            if (SetProperty(ref isEmailConfirm, value, nameof(IsEmailConfirm)))
            {
                //OnPropertyChanged(nameof(IsEmailConfirm));
            }
        }
    }

    private bool rememberMe;
    public bool RememberMe
    {
        get => rememberMe;
        set
        {
            if (SetProperty(ref rememberMe, value, nameof(RememberMe)))
            {
                //OnPropertyChanged(nameof(RememberMe));
            }
        }
    }

    [SQLite.Ignore]
    public string Token { get; set; } = "";

}

public partial class RegisterUser : User
{
    private string password = "";
    public string Password
    {
        get => password;
        set
        {
            if (SetProperty(ref password, value, nameof(Password)))
            {
                //OnPropertyChanged(nameof(Password));
            }
        }
    }
}

public partial class LoginUser : ObservableObject
{
    private string email = "";
    public string Email
    {
        get => email;
        set
        {
            if (SetProperty(ref email, value, nameof(Email)))
            {
                //OnPropertyChanged(nameof(Email));
            }
        }
    }
    private string password = "";
    public string Password
    {
        get => password;
        set
        {
            if (SetProperty(ref password, value, nameof(Password)))
            {
                //OnPropertyChanged(nameof(Password));
            }
        }
    }
    private bool rememberMe;
    public bool RememberMe
    {
        get => rememberMe;
        set
        {
            if (SetProperty(ref rememberMe, value, nameof(RememberMe)))
            {
                //OnPropertyChanged(nameof(RememberMe));
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


