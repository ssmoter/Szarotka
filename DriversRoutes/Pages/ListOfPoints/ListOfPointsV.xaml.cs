namespace DriversRoutes.Pages.ListOfPoints;

public partial class ListOfPointsV : ContentPage
{
    public ListOfPointsV(ListOfPointsVM vm)
    {
        InitializeComponent();
        vm.CalculateRoute = Map.CalculateRoute;
        BindingContext = vm;

        void StartRotation()
        {
            OutlineSyncImage.Rotation = 0;
            OutlineSyncImage.Animate("RotateIcon", new Animation(
                callback: d => OutlineSyncImage.Rotation = d,
                start: 0,
                end: 360
            ), length: 1000, easing: Easing.Linear, finished: (v, c) =>
            {
                if (!c) StartRotation();
            });
        }
        StartRotation();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
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
            vm.GetPointsFireAndForget(vm.Route, new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes());
            vm.SaveData = false;
        }
    }



}