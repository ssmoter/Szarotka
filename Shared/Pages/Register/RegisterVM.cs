using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Translated;
using DataBase.Validation;

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


        private readonly IAccessDataBaseAoT _db;
        private readonly IRegisterHttp _registerHttp;
        private readonly IUserValidation _userValidation;
        public RegisterVM(IAccessDataBaseAoT db, IRegisterHttp registerHttp, IUserValidation userValidation)
        {
            RegisterUser = new();
            RegisterM = new();
            _db = db;
            GetOldDrivers();
            _registerHttp = registerHttp;
            _userValidation = userValidation;
        }

        private void GetOldDrivers()
        {
            try
            {
                var dri = _db.DataBase.Table<Driver>().ToArray();
                Drivers = new ObservableCollection<Driver>(dri);
            }
            catch (Exception)
            {
                Drivers = [];
            }
        }

        public void IsPasswordEqual()
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
            bool flowControl = Validation();
            if (!flowControl)
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
                    RegisterM.EmailError = ex.ValidationErrors[i].Validation.EmailValidation(RegisterM.EmailError);
                    RegisterM.PasswordError = ex.ValidationErrors[i].Validation.PasswordsValidation(RegisterM.EmailError);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        internal bool Validation()
        {
            RegisterM.EmailError = "";
            RegisterM.PasswordError = "";

            bool emailOrPasswordIsNull = false;


            if (_userValidation.EmailValidFormat(RegisterUser.Email) == ServerEnums.Result.Error)
            {
                RegisterM.EmailError = EnumsList.Validation.EmailValidFormat.EmailValidation(RegisterM.EmailError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.EmailIsNull(RegisterUser.Email) == ServerEnums.Result.Error)
            {
                RegisterM.EmailError = EnumsList.Validation.EmailIsNull.EmailValidation(RegisterM.EmailError);
                emailOrPasswordIsNull = true;
            }

            if (_userValidation.PasswordContainEmail(RegisterUser.Password, RegisterUser.Email) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordContainEmail.PasswordsValidation(RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordLength8(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordLength8.PasswordsValidation(RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordNoDigit(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordNoDigit.PasswordsValidation(RegisterM.PasswordError); 
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordNoLower(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordNoLower.PasswordsValidation(RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordNoSpecial(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordNoSpecial.PasswordsValidation(RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordNoUpper(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordNoUpper.PasswordsValidation(RegisterM.PasswordError);
                emailOrPasswordIsNull = true;
            }
            if (_userValidation.PasswordIsNull(RegisterUser.Password) == ServerEnums.Result.Error)
            {
                RegisterM.PasswordError = EnumsList.Validation.PasswordIsNull.PasswordsValidation(RegisterM.PasswordError);
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
                return false;
            }

            return true;
        }
    }
}
