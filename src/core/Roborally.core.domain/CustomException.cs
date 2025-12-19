namespace Roborally.core.domain;

public class CustomException : Exception
{
    public int StatusCode { get; }

/// <author name="Sachin Baral 2025-09-16 11:52:08 +0200 7" />
    public CustomException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }

/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 12" />
    public CustomException(string message, Exception innerException, int statusCode = 500)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}