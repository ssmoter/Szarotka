using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Data.ServerHttpClients;

using System.Collections.ObjectModel;

namespace Shared.Pages.Register
{
    public partial class RegisterVM : ObservableObject
    {
        private RegisterUser registerUser;
        public RegisterUser RegisterUser
        {
            get => registerUser;
            set
            {
                if (SetProperty(ref registerUser, value))
                {
                    OnPropertyChanged(nameof(RegisterUser));
                }
            }
        }

        private RegisterM registerM;
        public RegisterM RegisterM
        {
            get => registerM;
            set
            {
                if (SetProperty(ref registerM, value))
                {
                    OnPropertyChanged(nameof(RegisterM));
                }
            }
        }

        private ObservableCollection<Driver> drivers;
        public ObservableCollection<Driver> Drivers
        {
            get => drivers;
            set
            {
                if (SetProperty(ref drivers, value))
                {
                    OnPropertyChanged(nameof(Drivers));
                }
            }
        }


        private readonly IAccessDataBase _db;
        private readonly IRegisterHttp _registerHttp;
        public RegisterVM(IAccessDataBase db, IRegisterHttp registerHttp)
        {
            RegisterUser = new();
            RegisterM = new();
            _db = db;
            var dri = _db.DataBase.Table<Driver>().ToArray();
            Drivers = new ObservableCollection<Driver>(dri);
            _registerHttp = registerHttp;
        }



        #region Methods
        public void IsPasswordEquels()
        {
            if (RegisterUser.Password != RegisterM.ConfirmPassword)
            {
                RegisterM.PasswordEquels = true;
            }
            else
            {
                RegisterM.PasswordEquels = false;
            }
        }

        private static string PasswordsValidation(EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.PasswordIsNull && valid <= EnumsList.Validation.PasswordContainEmail)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }

        private static string EmailValidation(EnumsList.Validation valid, string message)
        {
            if (valid >= EnumsList.Validation.EmailValidFormat && valid <= EnumsList.Validation.EmailIsNull)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    message += Environment.NewLine;
                }
                message += valid.ValidationToPolish();
            }
            return message;
        }

        #endregion
        #region Command

        [RelayCommand]
        void SetSelectedDriver(Driver driver)
        {
            if (RegisterUser.Id == driver.Id)
            {
                RegisterUser.Id = Guid.Empty;
                RegisterUser.Name = "";
                RegisterUser.Description = "";
                RegisterUser.Created = new DateTime();
            }
            else
            {
                RegisterUser.Id = new Guid(driver.Id.ToByteArray());
                RegisterUser.Name = driver.Name;
                RegisterUser.Description = driver.Description;
                RegisterUser.Created = driver.Created;
            }
        }

        [RelayCommand]
        async Task CreatedNewAccount()
        {
            RegisterM.EmailError = "";
            RegisterM.PasswordError = "";

            bool emailOrPasswordIsNull = false;

            if (string.IsNullOrWhiteSpace(RegisterUser.Email))
            {
                RegisterM.EmailError = EmailValidation(EnumsList.Validation.EmailIsNull, RegisterM.EmailError);
                emailOrPasswordIsNull = true;
            }
            if (string.IsNullOrWhiteSpace(RegisterUser.Password))
            {
                RegisterM.PasswordError = PasswordsValidation(EnumsList.Validation.PasswordIsNull, RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (string.IsNullOrWhiteSpace(RegisterUser.Name))
            {
                emailOrPasswordIsNull = true;
            }
            if (RegisterM.PasswordEquels)
            {
                emailOrPasswordIsNull = true;
            }

            if (emailOrPasswordIsNull)
            {
                return;
            }
            try
            {

                var task = _registerHttp.PostNewAccount(RegisterUser);
                await task;
                if (task.IsCompletedSuccessfully)
                {
                    await Shell.Current.GoToAsync($"{nameof(ConfirmEmail.ConfirmEmailV)}");
                }
            }
            catch (ValidationExceptionClient ex)
            {
                for (int i = 0; i < ex.ValidationErrors.Length; i++)
                {
                    RegisterM.EmailError = EmailValidation(ex.ValidationErrors[i].Validation, RegisterM.EmailError);
                    RegisterM.PasswordError = PasswordsValidation(ex.ValidationErrors[i].Validation, RegisterM.EmailError);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        #endregion


    }
}
