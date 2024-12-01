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

    public class ValidationException : Exception
    {
        public List<string> ValidationErrors { get; } = [];

        public void AddError(string error)
        {
            ValidationErrors.Add(error);
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

    }
}
