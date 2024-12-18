using DataBase.Model.EntitiesServer;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Model
{
    [Serializable]
    public class ErrorException : Exception
    {
        public string Error { get; }
        public override string Message { get; }

        public ErrorException(string? error, string message) : base(message)
        {
            Error = error is not null ? error : "";
            Message = message;
        }
    }

    [Serializable]
    public class ValidationException : Exception
    {
        public List<Valid> ValidationErrors { get; } = [];

        public void AddError(Valid valid)
        {
            ValidationErrors.Add(valid);
        }
        public void AddError(string message, EnumsList.Validation valid)
        {
            ValidationErrors.Add(new Valid(message, valid));
        }
        public int Count => ValidationErrors.Count;

        public string GetError()
        {
            string json = JsonSerializer.Serialize([.. ValidationErrors], ValidJsonSerializerContext.Default.ValidArray);
            return json;
        }
        public class Valid
        {
            public string Message { get; set; } = "";
            public EnumsList.Validation Validation { get; set; }
            public Valid(string message, EnumsList.Validation valid)
            {
                Message = message;
                Validation = valid;
            }
            public Valid()
            { }

        }
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(ValidationException.Valid[]))]
    public partial class ValidJsonSerializerContext : JsonSerializerContext
    {

    }
}
