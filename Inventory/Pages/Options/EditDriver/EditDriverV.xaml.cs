namespace Inventory.Pages.Options.EditDriver;

public partial class EditDriverV : ContentPage
{
	public EditDriverV(EditDriverVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}