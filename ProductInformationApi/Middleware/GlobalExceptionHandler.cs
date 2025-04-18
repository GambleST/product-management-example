using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ProductInformationApi.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ArgumentException or ArgumentNullException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        logger.LogError(exception, $"Exception occured: {exception.Message}");

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = exception.GetType().Name,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = "An error occurred while processing your request"
        };

        context.Response.StatusCode = problemDetails.Status.Value;

        await context.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}