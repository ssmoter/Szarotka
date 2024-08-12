namespace DataBase.Pages.ExistingFiles;

public partial class ExistingFilesV : ContentPage
{
    readonly ExistingFilesVM _vm;

    public ExistingFilesV(ExistingFilesVM vm)
    {
        InitializeComponent();
        this._vm = vm;
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

#if ANDROID
        CheckPermissions();
#endif
    }

    private async void CheckPermissions()
    {
        var result = await Service.AndroidPermissionService.CheckAllPermissionsAboutStorage();
        if (!result)
        {
            if (BindingContext is ExistingFilesVM vm)
            {
                await Shell.Current.GoToAsync($"../../{vm.ReturnPage}");
            }
        }
    }



    private void SwipeItem_Invoked_SelectedImport(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as ExistingFilesM;
        if (product == null) { return; }

        _vm.SelectedImportCommand.Execute(product);
    }

    private void SwipeItem_Invoked_SelectedExport(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        if (item is null) { return; }

        var product = item.BindingContext as ExistingFilesM;
        if (product == null) { return; }

        _vm.SelectedExportCommand.Execute(product);
    }
}