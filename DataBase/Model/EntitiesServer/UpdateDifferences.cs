using CommunityToolkit.Mvvm.ComponentModel;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesServer;

public partial class UpdateDifference : ObservableObject
{
    private IDifference? server;
    public IDifference? Server
    {
        get => server;
        set
        {
            if (SetProperty(ref server, value, nameof(Server)))
            {
            }
        }
    }
    private IDifference? update;
    public IDifference? Update
    {
        get => update;
        set
        {
            if (SetProperty(ref update, value, nameof(Update)))
            {
            }
        }
    }

    private int index;
    [JsonIgnore]
    public int Index
    {
        get => index;
        set
        {
            if (SetProperty(ref index, value, nameof(Index)))
            {
            }
        }
    }
    private object? updateSelect = null;
    [JsonIgnore]
    public object? UpdateSelect
    {
        get => updateSelect;
        set
        {
            if (SetProperty(ref updateSelect, value, nameof(UpdateSelect))) { }
        }
    }
}
