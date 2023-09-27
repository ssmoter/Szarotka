namespace Inventory.Pages.RangeDay.ExistingFiles;

public partial class ExistingFilesV : ContentPage
{
	public ExistingFilesV(ExistingFilesVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}