using System.Net;
using System.Text.Json;
using AccountHub.Api.Extensions;
using AccountHub.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "An error occurred while processing your request.",
            Detail = exception.Message
        };

        switch (exception)
        {
            case JsonException jsonException:
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Invalid JSON";
                problemDetails.Detail = "The provided JSON is invalid.";
                problemDetails.Extensions.Add("details", jsonException.Message);
                break;

            case DbUpdateConcurrencyException:
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                problemDetails.Title = "Concurrency Conflict";
                problemDetails.Detail = "The record has been modified by another user. Please refresh and try again.";
                break;

            case UnauthorizedAccessException:
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "Unauthorized Access";
                problemDetails.Detail = "You are not authorized to perform this action.";
                break;

            case BaseException baseException:
                problemDetails.Status = (int)baseException.HttpStatusCode;
                problemDetails.Title = baseException.Title;
                problemDetails.Detail = baseException.Message;
                if (baseException.Details is not null)
                {
                    problemDetails.Extensions.Add("details", baseException.Details);
                }
                break;

            default:
                _logger.LogError(exception, "An unhandled exception occurred");
                problemDetails.Extensions.Add("details", exception.StackTrace);
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}