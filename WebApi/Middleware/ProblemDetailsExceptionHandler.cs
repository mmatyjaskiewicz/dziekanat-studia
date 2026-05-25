using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Middleware;

// Globalny handler wyjątków - wykorzystuje IExceptionHandler.
// Zamienia wyjątki domenowe na odpowiedzi ProblemDetails.
public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory,
    ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is LecturerNotFoundException or StudentNotFoundException)
        {
            logger.Log(LogLevel.Information, $"Exception '{exception.Message}' handled!");

            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Service error!",
                "Service error",
                detail: exception.Message
            );
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        if (exception is KeyNotFoundException)
        {
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                "Resource not found",
                "Not found",
                detail: exception.Message
            );
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        return false;
    }
}
