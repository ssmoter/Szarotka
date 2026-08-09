using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Translated;

using Shared.Data;
using Shared.Data.ServerHttpClients;

namespace Shared.Pages.LogIn.ForgetPassword
{
    public partial class ForgetPasswordVM(IResetPasswordHttp resetPasswordHttp, IAccessDataBaseAoT db) : ObservableObject
    {
        private ForgetPasswordM model = new();
        public ForgetPasswordM Model
        {
            get => model; set
            {
                if (SetProperty(ref model, value, nameof(Model))) { }
            }
        }


        private readonly IAccessDataBaseAoT _db = db;
        private readonly IResetPasswordHttp _resetPasswordHttp = resetPasswordHttp;

        [RelayCommand]
        void IsPasswordEquals()
        {
            Model.IsPasswordEquals = true;
            if (Model.Password == Model.PasswordReaped)
            {
                Model.IsPasswordEquals = false;
            }
        }


        [RelayCommand]
        async Task SendEmail()
        {
            try
            {
                Model.EmailValid = "";
                var value = Model.Email;

                await _resetPasswordHttp.SendEmail(value);

                Model.IsSendCodeVisible = true;
                Model.IsEmailEnable = false;
                await Toast.Make("Email został wysłany").Show();
            }
            catch (ValidationExceptionClient ex)
            {
                var valid = "";
                foreach (var item in ex.ValidationErrors)
                {
                    valid += item.Validation.EmailValidation(valid);
                }
                Model.EmailValid = valid;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task CheckCode()
        {
            try
            {
                Model.CodeValid = "";
                var value = Model.Code;

                await _resetPasswordHttp.SendCode(value);

                await Toast.Make("Poprawny kod").Show();
                Model.IsPasswordVisible = true;
                Model.IsSendCodeEnable = false;
            }
            catch (ValidationExceptionClient ex)
            {
                var valid = "";
                foreach (var item in ex.ValidationErrors)
                {
                    valid += item.Validation.UnclassifiedValidation(valid);
                }
                Model.CodeValid = valid;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }


        [RelayCommand]
        async Task SendNewPassword()
        {
            try
            {
                Model.CodeValid = "";
                Model.PasswordValid = "";

                var value = Model.Password;
                var value2 = Model.Code;

                await _resetPasswordHttp.ResetPassword(value2, value);

                await Toast.Make("Hasło zresetowane").Show();
                Model = new();
                await Shell.Current.GoToAsync("..");
            }
            catch (ValidationExceptionClient ex)
            {
                var validCode = "";
                var validPassword = "";
                foreach (var item in ex.ValidationErrors)
                {
                    validCode += item.Validation.UnclassifiedValidation(validCode);
                    validPassword += item.Validation.PasswordsValidation(validCode);
                }
                Model.CodeValid = validCode;
                Model.PasswordValid = validPassword;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }
}
