using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

namespace Shared.Pages.UpdateDifference
{
    public partial class DifferenceDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Customer { get; set; }
        public DataTemplate Empty { get; set; }
        public DataTemplate PName { get; set; }
        public DataTemplate DayTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is DataBase.Model.EntitiesServer.UpdateDifference mixed)
            {
                return mixed.Server switch
                {
                    CustomerRoutes => Customer,
                    ProductName => PName,
                    Day => DayTemplate,
                    _ => Empty
                };
            }
            return null;
        }
    }
}
