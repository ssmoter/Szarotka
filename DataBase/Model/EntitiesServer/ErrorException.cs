using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesServer
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


    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(ValidationException.Valid[]))]
    public partial class ValidJsonSerializerContext : JsonSerializerContext
    { }


    public class ValidationExceptionClient : Exception
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public int Status { get; set; }
        public string Instance { get; set; } = "";
        public string TraceId { get; set; } = "";
        public string Requestid { get; set; } = "";
        public string Detail { get; set; } = "";
        public ValidationException.Valid[] ValidationErrors { get; set; } = [];

        public static Exception ThrowValidationException(string json)
        {
            ValidationExceptionClient? ex = null;
            try
            {
                ex = JsonSerializer.Deserialize(json, ValidationExceptionClientJsonSerializerContext.Default.ValidationExceptionClient);
                if (ex is not null)
                {
                    var errors = JsonSerializer.Deserialize(ex.Detail, ValidJsonSerializerContext.Default.ValidArray);
                    ex.ValidationErrors = errors is not null ? errors : [];
                }
            }
            catch (Exception)
            { }
            if (ex is not null)
            {
                return ex;
            }
            return new Exception(json);
        }
    }
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(ValidationExceptionClient))]
    public partial class ValidationExceptionClientJsonSerializerContext : JsonSerializerContext
    {

    }

}
