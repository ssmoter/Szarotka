namespace DriversRoutes.Pages.ListOfPoints;

public partial class ListOfPointsV : ContentPage
{
    public ListOfPointsV(ListOfPointsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is ListOfPointsVM vm)
        {
            var result = await vm.GetPointsAsync(vm.Route, new Model.SelectedDayOfWeekRoutes());
            if (result != vm.CustomerRoutes)
            {
                vm.CustomerRoutes.Clear();
                vm.CustomerRoutes = result;
            }

        }
    }
}