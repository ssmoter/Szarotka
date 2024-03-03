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
            if (vm.Route is null)
            {
                return;
            }

            if (vm.CustomerRoutes.Count > 1)
            {
                if (vm.CustomerRoutes.FirstOrDefault().RoutesId == vm.Route.Id)
                {
                    return;
                }
            }
            var result = await vm.GetPointsAsync(vm.Route, new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes());

            vm.CustomerRoutes.Clear();
            vm.CustomerRoutes = result;
        }
    }



}