using DataBase.Model.EntitiesServer;

using System.Text;

namespace Server.Model
{
    [Serializable]
    public class ErrorException : Exception
    {
        public string Error { get; }
        public override string Message { get; }

        public ErrorException(string error, string message) : base(message)
        {
            Error = error;
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
            StringBuilder error = new();
            for (int i = 0; i < ValidationErrors.Count; i++)
            {
                error.Append(i + 1);
                error.Append(':');
                error.Append(' ');
                error.Append(ValidationErrors[i]);
                error.Append(Environment.NewLine);
            }
            return error.ToString();
        }

        [Serializable]
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
}
