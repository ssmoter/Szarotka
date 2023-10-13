namespace DriversRoutes.Pages.ListOfPoints;

public partial class ListOfPointsV : ContentPage
{
    public ListOfPointsV(ListOfPointsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is ListOfPointsVM vm)
        {
            vm.CustomerRoutes = vm.GetPoints(vm.Route, new Model.SelectedDayOfWeekRoutes());
        }
    }
}