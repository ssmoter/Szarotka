using System.Text.Json.Serialization;

namespace DriversRoutes.Model.Route
{

    [JsonSerializable(typeof(ComputeRouteMatrixRequest))]

    [JsonSerializable(typeof(Response))]

    [JsonSerializable(typeof(ComputeRoutesRequest))]

    [JsonSourceGenerationOptions(WriteIndented = true,
                UseStringEnumConverter = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    public partial class GoogleApiJsonSerializerContext : JsonSerializerContext
    {
    }
}
