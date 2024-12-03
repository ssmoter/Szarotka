using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBase.Model.EntitiesRoutes
{
    public partial class Routes : BaseEntities<Guid>
    {
        [ObservableProperty]
        string name = "";
    }
}
