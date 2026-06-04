using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Pages.UserDisplay.PopupUser;
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
                    Task.Run(async () =>
                    {
                        var description = await userDisplayVPopup.GetUser(_user.Id);
                        User.Description = description.Description;
                    });
                }
            }
        }

        private User user;
        public User User
        {
            get => user;
            set => SetProperty(ref user, value, nameof(User));
        }

        private readonly UserDisplayVPopup userDisplayVPopup = new();
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
