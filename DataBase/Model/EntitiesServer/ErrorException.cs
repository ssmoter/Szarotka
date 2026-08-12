using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesServer
{
    [Serializable]
    public class ErrorException(string? error, string message) : Exception(message)
    {
        public string Error { get; } = error is not null ? error : "";
        public override string Message { get; } = message;
    }

    public interface IValidationException
    {
        IList<ValidationException.Valid> ValidationErrors { get; }

        void AddError(string message, EnumsList.Validation valid);
        void AddError(ValidationException.Valid valid);
        string GetError();
        void Throw();
    }

    [Serializable]
    public class ValidationException : Exception, IValidationException
    {
        public IList<Valid> ValidationErrors { get; private set; } = [];

        public void AddError(Valid valid)
        {
            ValidationErrors ??= [];
            ValidationErrors.Add(valid);
        }
        public void AddError(string message, EnumsList.Validation valid)
        {
            ValidationErrors ??= [];
            ValidationErrors.Add(new Valid(message, valid));
        }
        public string GetError()
        {
            string json = JsonSerializer.Serialize([.. ValidationErrors], ValidJsonSerializerContext.Default.ValidArray);
            return json;
        }
        public void Throw()
        {
            if (ValidationErrors?.Count > 0)
            {
                throw this;
            }
        }


        public ValidationException()
        { }

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
    public static class ValidationExceptionExtension
    {
        public static void Throw(this ValidationException valid)
        {
            if (valid?.ValidationErrors?.Count > 0)
            {
                throw valid;
            }
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
        public string RequestId { get; set; } = "";
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
                    return ex;
                }
            }
            catch (Exception)
            { }
            return new Exception(json);
        }
    }
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(ValidationExceptionClient))]
    public partial class ValidationExceptionClientJsonSerializerContext : JsonSerializerContext
    {

    }

}
