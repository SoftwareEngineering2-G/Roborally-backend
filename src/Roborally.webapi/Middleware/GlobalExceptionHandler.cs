using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Roborally.core.application;
using Roborally.core.domain;

namespace Roborally.webapi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = 500;

        if (exception is CustomException customException)
        {
            statusCode = customException.StatusCode;
        }

        ProblemDetails problemDetails = new ProblemDetails()
        {
            Title = statusCode == 500 ? "Internal server error" : exception.Message,
            Status = statusCode,
            Detail = exception.Message,
        };
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}