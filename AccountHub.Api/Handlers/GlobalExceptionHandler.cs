using System.Net;
using AccountHub.Api.Extensions;
using AccountHub.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Api.Handlers;

public class GlobalExceptionHandler:IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BaseException baseException)
            throw exception;


        var problemDetails = new ProblemDetails
        {
            Status = (int)baseException.HttpStatusCode,
            Title = baseException.Title,
            Detail = baseException.Message
        };

        if (baseException.Details is not null)
        {
            problemDetails.Extensions.Add("details", baseException.Details);
        }


        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}