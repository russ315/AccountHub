using AccountHub.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Handlers;

public class GlobalExceptionHandler:IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var baseException = exception as BaseException;
        if (baseException == null)
            throw new SystemException();
        var problemDetails = new ProblemDetails
        {
            Status = (int)baseException.HttpStatusCode,
            Detail = baseException.Message,
            Title = baseException.Title,
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}