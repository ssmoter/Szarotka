namespace DataBase.Model.EntitiesRoutes;

public partial class Routes : BaseEntities<Guid>
{
    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name))) { }
        }
    }
}

