using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Pages.UserDisplay.UserEdit;

namespace Shared.Pages.UserDisplay
{
    public partial class UserDisplayVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(User), out object user))
            {
                if (user is User _user)
                {
                    User = _user;
                }
            }
        }

        private User user;
        public User User
        {
            get => user;
            set => SetProperty(ref user, value, nameof(User));
        }

        public UserDisplayVM()
        {
            User ??= new();
        }

        [RelayCommand]
        async Task GoToEdit()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(User), User }
            };
            await Shell.Current.GoToAsync(nameof(UserEditV), navigationParameter);
        }


    }
}
