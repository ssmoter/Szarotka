using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Translated;

using Shared.Data;
using Shared.Data.ServerHttpClients;

namespace Shared.Pages.ConfirmEmail
{
    public partial class ConfirmEmailVM : ObservableObject
    {
        private ConfirmEmailM confirmEmailM;
        public ConfirmEmailM ConfirmEmailM
        {
            get => confirmEmailM;
            set
            {
                if (SetProperty(ref confirmEmailM, value))
                {
                    OnPropertyChanged(nameof(ConfirmEmailM));
                }
            }
        }

        private readonly IAccessDataBaseAoT _db;
        private readonly IRegisterHttp _registerHttp;
        public ConfirmEmailVM(IAccessDataBaseAoT db, IRegisterHttp registerHttp)
        {
            ConfirmEmailM = new();
            _db = db;
            _registerHttp = registerHttp;
        }


        string CodeValid(EnumsList.Validation valid, string error)
        {
            if (valid >= EnumsList.Validation.CodeIsExpire && valid <= EnumsList.Validation.CodeNotExist)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    error += Environment.NewLine;
                }
                error += valid.ValidationToPolish();
            }
            return error;
        }

        [RelayCommand]
        async Task SendCode()
        {
            var code = ConfirmEmailM.Code.ToString();
            ConfirmEmailM.Error = "";
            if (code.Length != 5)
            {
                return;
            }

            try
            {
                var task = _registerHttp.ConfirmEmail(code);
                await task;

                if (task.IsCompletedSuccessfully)
                {
                    var user = task.Result;
                    await Shell.Current.GoToAsync($"../{nameof(LogIn.LogInV)}?{nameof(User.Email)}={user.Email}");
                }
            }
            catch (ValidationExceptionClient valid)
            {
                for (int i = 0; i < valid.ValidationErrors.Length; i++)
                {
                    ConfirmEmailM.Error += CodeValid(valid.ValidationErrors[i].Validation, ConfirmEmailM.Error);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }

    }
}
