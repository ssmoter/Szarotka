using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;
using Shared.Model;
using Shared.Pages.ConfirmEmail;

using System.Web;

namespace Shared.Pages.LogIn
{
    public partial class LogInVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.TryGetValue(nameof(LoginUser.Email), out object email))
            {
                if (email is not null)
                {
                    User.Email = HttpUtility.UrlDecode(email.ToString());
                    Errors = "";
                }
            }
        }

        private LoginUser user;
        public LoginUser User
        {
            get => user;
            set
            {
                if (SetProperty(ref user, value))
                {
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        private string errors = "";
        public string Errors
        {
            get => errors;
            set
            {
                if (SetProperty(ref errors, value))
                {
                    OnPropertyChanged(nameof(Errors));
                }
            }
        }
        private bool isPassword;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                if (SetProperty(ref isPassword, value))
                {
                    OnPropertyChanged(nameof(IsPassword));
                }
            }
        }


        private readonly AccessDataBase _db;
        private readonly ILoginHttp _loginHttp;
        public LogInVM(AccessDataBase db, ILoginHttp loginHttp)
        {
            User = new();
            _db = db;
            _loginHttp = loginHttp;

#if DEBUG
            User.Email = "user@example.com";
            User.Password = "Password1!";
#endif
        }

        static string Valid(EnumsList.Validation valid, string error)
        {
            if (valid == EnumsList.Validation.LoginIsNull
                || valid == EnumsList.Validation.PasswordIsNull
                || valid == EnumsList.Validation.EmailIsNull)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    error += Environment.NewLine;
                }
                error += valid.ValidationToPolish();
            }
            else if (valid >= EnumsList.Validation.AccountNotFound && valid <= EnumsList.Validation.AccountEmailIsNotConfirm)
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
        async Task GotoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Register.RegisterV));
        }


        [RelayCommand]
        async Task LogIn()
        {
            Errors = "";
            try
            {
                var result = await _loginHttp.In(User);

                Helper.UserAfterLogin.SetLoginUser(result);

                var toast = Toast.Make($"Zalogowano {UserAfterLogin.User.Name}", duration: CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();

                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.GoToAsync("MainPage");
                }

            }
            catch (ValidationExceptionClient ex)
            {
                for (int i = 0; i < ex.ValidationErrors.Length; i++)
                {
                    Errors = Valid(ex.ValidationErrors[i].Validation, Errors);
                }

                if (ex.ValidationErrors.Any(x => x.Validation == EnumsList.Validation.AccountEmailIsNotConfirm))
                {
                    var toast = Toast.Make("Potwierdź maila w celu zalogowania się", duration: CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                    await Shell.Current.GoToAsync(nameof(ConfirmEmailV));
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }
}
