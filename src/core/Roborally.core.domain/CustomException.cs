namespace Roborally.core.application;

public class CustomException : Exception {
    public int StatusCode { get; }

    public CustomException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }

    public CustomException(string message, Exception innerException, int statusCode = 500) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}