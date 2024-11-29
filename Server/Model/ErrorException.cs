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



    }
}
