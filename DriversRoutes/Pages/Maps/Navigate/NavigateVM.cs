using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.CustomControls;
using DataBase.Data;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Pages.Maps.Controls;

using Microsoft.Maui.Maps;

using System.Collections.ObjectModel;


namespace DriversRoutes.Pages.Maps.Navigate
{
    [QueryProperty(nameof(AllPoints), nameof(ObservableCollection<CustomerRoutes>))]
    [QueryProperty(nameof(SelectedPoint), nameof(CustomerRoutes))]
    public partial class NavigateVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<CustomerRoutes> allPoints;

        [ObservableProperty]
        CustomerRoutes selectedPoint;

        [ObservableProperty]
        StepSelected stepSelected = StepSelected.One;

        [ObservableProperty]
        bool isTrafficEnabled;
        [ObservableProperty]
        MapType mapType;
        private readonly DataBase.Data.AccessDataBase _db;

        public NavigateVM(AccessDataBase db)
        {
            _db = db;
        }

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
        void ChangeTypeOfMap(string type)
        {
            if (int.TryParse(type, result: out int result))
            {
                if (result >= 0 && result <= 2)
                {
                    MapType = (MapType)result;
                }
            }
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
                _db.SaveLog(ex);
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
        void FitMapToMarkers()
        {
            BlazorMap.OnFitMapToAdvancedMarkers();
        }

    }
}
