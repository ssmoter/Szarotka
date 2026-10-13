using DataBase.Data;
using DataBase.Model;

using Shared.Data;
using Shared.Helper;
using Shared.Service;

using System.Net;

namespace Shared.Pages.LoadingPage;

public partial class LoadingPageV : ContentPage
{
    private readonly ICreatedDataBase _createdDataBase;
    private readonly IAccessDataBaseAoT _db;
    public LoadingPageV(ICreatedDataBase createdDataBase, IAccessDataBaseAoT db)
    {
        InitializeComponent();
        _createdDataBase = createdDataBase;
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await Task.Yield();
            await Task.Run(async () =>
            {
                var update = await UpdateDataBase();
                if (update)
                {
                    await GotToLogin();
                }
            });
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }


    private async Task GotToLogin()
    {
        try
        {
            //nie działa
            //_db.DbSyncAoT.Execute("PRAGMA wal_checkpoint(PASSIVE);"); 
            await UserAfterLogin.SetLoginUser();
            if (!UserAfterLogin.IsLogin)
            {
                await SaveTaskGoTo($"{nameof(Shared.Pages.LogIn.LogInV)}");
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }
    private async Task<bool> UpdateDataBase()
    {
        var result = true;
        try
        {
            var old = _createdDataBase.GetCurrentVersion();
            if (!old.Equals(new DataBaseVersion()))
            {
                await SaveTaskGoTo(nameof(Shared.Pages.UpdateDataBase.UpdateDataBaseV));
                result = false;
            }
            await _createdDataBase.CreateBackUp();
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        return result;
    }
    private static async Task SaveTaskGoTo(string page)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                await Shell.Current.GoToAsync(page);
            }
            catch (Exception navEx)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd nawigacji: {navEx.Message}");
            }
        });
    }

}