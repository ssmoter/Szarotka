using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Pages.Maps.Controls;

using Shared.CustomControls;
using Shared.Data;

using System.Collections.ObjectModel;


namespace DriversRoutes.Pages.Maps.Navigate;
public partial class NavigateVM(IAccessDataBase db) : ObservableObject, IQueryAttributable
{

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(ObservableCollection<CustomerRoutes>), out object allpoints))
        {
            if (allpoints is ObservableCollection<CustomerRoutes> _allpoints)
            {
                AllPoints = _allpoints;
            }
        }
        if (query.TryGetValue(nameof(CustomerRoutes), out object selectedPoint))
        {
            if (selectedPoint is CustomerRoutes _selectedPoint)
            {
                SelectedPoint = _selectedPoint;
            }
        }
    }
    private ObservableCollection<CustomerRoutes> allPoints;
    public ObservableCollection<CustomerRoutes> AllPoints
    {
        get => allPoints;
        set
        {
            if (SetProperty(ref allPoints, value, nameof(AllPoints))) { }
        }
    }

    private CustomerRoutes selectedPoint;
    public CustomerRoutes SelectedPoint
    {
        get => selectedPoint;
        set
        {
            if (SetProperty(ref selectedPoint, value, nameof(SelectedPoint))) { }
        }
    }

    private StepSelected stepSelected = Shared.CustomControls.StepSelected.One;
    public StepSelected StepSelected
    {
        get => stepSelected;
        set
        {
            if (SetProperty(ref stepSelected, value, nameof(StepSelected))) { }
        }
    }

    private readonly IAccessDataBase _db = db;

    private void DescriptionOfPreviousPoint(int direction)
    {
        int index;
        index = SelectedPoint.QueueNumber + direction;
        if (index > AllPoints.Count)
        {
            index = 1;
        }
        else if (index < 1)
        {
            index = AllPoints.Count;
        }
        BlazorMap.OnRemoveAdvancedMarker(SelectedPoint);
        SelectedPoint = AllPoints.FirstOrDefault(x => x.QueueNumber == index);
        if (SelectedPoint is null)
        {
            return;
        }
        BlazorMap.OnSetCustomer(SelectedPoint);
        BlazorMap.OnSetAdvancedMarker();
        BlazorMap.OnRemoveDrirections();
        BlazorMap.OnFitMapToAdvancedMarkers();
    }


    [RelayCommand]
    void ShowMovingView()
    {
        StepSelected = MovingViewInSteps.StepUp(StepSelected);
    }
    [RelayCommand]
    void HideMovingView()
    {
        StepSelected = MovingViewInSteps.StepDown(StepSelected);
    }
    [RelayCommand]
    void DisplayDescriptionOfNextPoint()
    {
        DescriptionOfPreviousPoint(1);
    }
    [RelayCommand]
    void DisplayDescriptionOfPreviousPoint()
    {
        DescriptionOfPreviousPoint(-1);
    }
    [RelayCommand]
    async Task DisplayDescriptionPin(CustomerRoutes point)
    {
        try
        {
            if (point is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(Pages.Customer.DisplayCustomer.DisplayCustomerV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(CustomerRoutes)] = point
                });
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
    }
    [RelayCommand]
    void CalculateRoute()
    {
        if (SelectedPoint is null)
        {
            return;
        }
        Task.Run(async () =>
        {
            await BlazorMap.OnAddDirections();
        });
    }
    [RelayCommand]
    static void FitMapToMarkers()
    {
        BlazorMap.OnFitMapToAdvancedMarkers();
    }

}

