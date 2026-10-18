using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

using System.Runtime.CompilerServices;

namespace Shared.CustomControls
{
    public partial class DisplayRouteName : Label
    {

        public static readonly BindableProperty IdRouteProperty =
            BindableProperty.Create(
                nameof(IdRoute),
                typeof(Guid),
                typeof(DisplayRouteName),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    SetName(bindable);
                });

        private static void SetName(BindableObject bindable)
        {
            if (bindable is DisplayRouteName displayRouteName)
            {
                var route = _routes.FirstOrDefault(x => x.Id == displayRouteName.IdRoute);
                if (!displayRouteName.OnlyRoute)
                {
                    displayRouteName.Text = $"Trasa: {route?.Name}" ?? "Unknown Route";
                }
                else
                {
                    displayRouteName.Text = $"{route?.Name}" ?? "Unknown Route";
                    displayRouteName.Style = null;
                }
            }
        }

        public Guid IdRoute
        {
            get => (Guid)GetValue(IdRouteProperty);
            set => SetValue(IdRouteProperty, value);
        }

        public static readonly BindableProperty OnlyRouteProperty =
    BindableProperty.Create(
        nameof(OnlyRoute),
        typeof(bool),
        typeof(DisplayRouteName),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            SetName(bindable);
        });



        public bool OnlyRoute
        {
            get => (bool)GetValue(OnlyRouteProperty);
            set => SetValue(OnlyRouteProperty, value);
        }


        private static IList<Routes> _routes = [];
        private readonly IGetDriverRoutesAoT _getDriverRoutesAoT;

        public DisplayRouteName()
        {
            _getDriverRoutesAoT = Shared.Service.AppServiceProvider.Current.GetService<IGetDriverRoutesAoT>();
            if (!_routes.Any())
            {
                Task.Run(async () =>
                {
                    _routes = await _getDriverRoutesAoT.Routes();
                    SetName(this);
                });
            }
            Style = (Style)Application.Current.Resources["LabelPointerOver"];
        }






    }
}
