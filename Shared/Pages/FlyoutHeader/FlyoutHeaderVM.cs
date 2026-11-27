using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace Shared.Pages.FlyoutHeader
{
    public partial class FlyoutHeaderVM : ObservableObject, IDisposable
    {
        private bool isLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                if (SetProperty(ref isLogin, value))
                {
                    OnPropertyChanged(nameof(IsLogin));
                }
            }
        }
        private User user = new();
        public User User
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

        private IView customContent;
        public IView CustomContent
        {
            get => customContent;
            set
            {
                if (SetProperty(ref customContent, value, nameof(CustomContent))) { }
            }
        }
        public static List<ToolbarItem> ToolbarItems { get; set; } = [];
        public static Action<ToolbarItem> ActionToolbarItemSet;
        public static Action<ToolbarItem> ActionToolbarItemRemove;
        private void _OnSetToolbarItem(ToolbarItem item)
        {
            ToolbarItems ??= [];
            ToolbarItems.Add(item);
        }
        public static void SetToolbarItem(ToolbarItem item)
        {
            ActionToolbarItemSet?.Invoke(item);
        }
        private void _RemoveSetToolbarItem(ToolbarItem item)
        {
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(() => ToolbarItems.Remove(item));
                return;
            }
        }
        public static void RemoveToolbarItem(ToolbarItem item)
        {
            ActionToolbarItemRemove?.Invoke(item);
        }

        private static Func<bool> funcCustomContentExist;
        private static Action<IView> actionCustomContent;
        public static void OnCustomContent(IView view = null)
        {
            actionCustomContent?.Invoke(view);
        }
        /// <summary>
        /// Sprawdza czy została dodana jakaś zawartość.
        /// Zwraca false jeżeli content jest null.
        /// </summary>
        /// <returns>Zwraca false jeżeli content jest null</returns>
        public static bool IsCustomContentExist()
        {
            return (bool)funcCustomContentExist?.Invoke();
        }

        private bool isCustomContentExist()
        {
            if (CustomContent is null)
            {
                return false;
            }
            return true;
        }


        private CancellationTokenSource? _fadeOutCts;

        private void _onCustomContent(IView? view)
        {
            // 1. Zawsze wymuszaj wykonanie na głównym wątku UI
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(() => _onCustomContent(view));
                return;
            }

            // 2. Anuluj poprzednią animację ukrywania, jeśli trwa (zapobiega wyścigom)
            _fadeOutCts?.Cancel();
            _fadeOutCts?.Dispose();
            _fadeOutCts = null;

            if (view is null)
            {
                if (CustomContent is not null)
                {
                    _fadeOutCts = new CancellationTokenSource();
                    _ = FadeOutElementSafeAsync(CustomContent, _fadeOutCts.Token);
                }
            }
            else
            {
                // Zabezpieczenie przed brakiem aktywnego Shell lub zamkniętym oknem
                if (Shell.Current?.Handler is not null)
                {
                    try
                    {
                        Shell.Current.FlyoutIsPresented = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Błąd otwierania Flyout: {ex.Message}");
                    }
                }

                CustomContent = view;
            }
        }

        private async Task FadeOutElementSafeAsync(IView view, CancellationToken token)
        {
            try
            {
                // Czekaj 2 sekundy, ale przerwij natychmiast, jeśli pojawi się nowy widok
                await Task.Delay(TimeSpan.FromSeconds(2), token);

                if (token.IsCancellationRequested)
                    return;

                await FadeOutElement(view);

                if (!token.IsCancellationRequested && ReferenceEquals(CustomContent, view))
                {
                    CustomContent = null;
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorujemy – animacja została celowo anulowana przez nowy komunikat
            }
            catch (Exception ex)
            {
                // Nigdy nie rzucamy wyjątku w metodzie "fire-and-forget" (unikamy crashu aplikacji)
                System.Diagnostics.Debug.WriteLine($"Błąd animacji ukrywania: {ex.Message}");
            }
        }

        private async Task FadeOutElement(IView view)
        {
            // Sprawdzamy, czy element nadal jest podpięty do drzewa wizualnego (Handler != null)
            if (view is VisualElement element && element.Handler is not null)
            {
                await Task.WhenAll(
                    element.FadeToAsync(0, 500),
                    element.TranslateToAsync(0, -20, 500)
                );
            }
        }


        public FlyoutHeaderVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;

            actionCustomContent += _onCustomContent;
            funcCustomContentExist += isCustomContentExist;

            ActionToolbarItemSet += _OnSetToolbarItem;
            ActionToolbarItemRemove += _RemoveSetToolbarItem;
        }

        private void UserAfterLogin_OnLogin(User user, bool arg2)
        {
            User = user;
            IsLogin = arg2;
        }

        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync(nameof(LogIn.LogInV));
        }

        [RelayCommand]
        async Task GoToUser()
        {
            var user = User;
            await Shell.Current.GoToAsync($"{nameof(UserDisplay.UserDisplayV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(UserDisplay.UserDisplayVM.User)] = user
                });
        }

        public void Dispose()
        {
            UserAfterLogin.OnLogin -= UserAfterLogin_OnLogin;
            actionCustomContent -= _onCustomContent;
            ActionToolbarItemSet -= _OnSetToolbarItem;
            ActionToolbarItemRemove -= _RemoveSetToolbarItem;
            funcCustomContentExist -= isCustomContentExist;

        }
    }
}

