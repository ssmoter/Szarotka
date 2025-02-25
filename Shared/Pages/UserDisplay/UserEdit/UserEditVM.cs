using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Translated;

using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;

namespace Shared.Pages.UserDisplay.UserEdit
{
    public partial class UserEditVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(User), out object user))
            {
                if (user is User _user)
                {
                    Old = _user;
                    Edit = new User(_user);
                }
            }
        }

        private User old;
        public User Old
        {
            get => old;
            set
            {
                if (SetProperty(ref old, value, nameof(Old)))
                { }
            }
        }
        private User edit;
        public User Edit
        {
            get => edit;
            set
            {
                if (SetProperty(ref edit, value, nameof(Edit)))
                { }
            }
        }

        private UserEditM userEditM = new();
        public UserEditM UserEditM
        {
            get => userEditM;
            set
            {
                if (SetProperty(ref userEditM, value, nameof(UserEditM))) { }
            }
        }


        private double maxMyWidth;
        public double MaxMyWidth
        {
            get => maxMyWidth;
            set
            {
                if (SetProperty(ref maxMyWidth, value, nameof(MaxMyWidth))) { }
            }
        }

        private UserTypeAndFriendlyName[] userTypes;
        public UserTypeAndFriendlyName[] UserTypes
        {
            get => userTypes;
            set
            {
                if (SetProperty(ref userTypes, value, nameof(UserTypes))) { }
            }
        }

        private readonly IAccessDataBase _db;
        private readonly IEditUserHttp _editUserHttp;
        public UserEditVM(IAccessDataBase db, IEditUserHttp editUserHttp)
        {
            Edit = new User();
            Old = new User();
            UserTypes = new UserTypeAndFriendlyName[3];
            UserTypes[0] = (UserType.Driver.ToFriendlyString(), UserType.Driver);
            UserTypes[1] = (UserType.Confectioner.ToFriendlyString(), UserType.Confectioner);
            UserTypes[2] = (UserType.Baker.ToFriendlyString(), UserType.Baker);
            _db = db;
            _editUserHttp = editUserHttp;
        }

        [RelayCommand]
        async Task Cancel()
        {
            Edit = new();
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        async Task Save()
        {
            try
            {
                UserEditM = new();
                var user = Edit;
                var result = await _editUserHttp.EditUser(user);
                Old = result;
                Edit = new(result);
                UserAfterLogin.SetLoginUser(result);

                await Toast.Make("Edytowano").Show();
            }
            catch (ValidationExceptionClient ex)
            {
                foreach (var item in ex.ValidationErrors)
                {
                    UserEditM.EmailError = item.Validation.EmailValidation(UserEditM.EmailError);
                    UserEditM.NameError = item.Validation.NameValidation(UserEditM.NameError);
                    UserEditM.PhoneError = item.Validation.PhoneNumberValidation(UserEditM.PhoneError);
                    UserEditM.Error = item.Validation.UnclassifiedValidation(UserEditM.Error);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }


}
