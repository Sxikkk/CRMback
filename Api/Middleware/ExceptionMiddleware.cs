using System.Net;
using System.Text.Json;
using Domain.Exceptions;

namespace Api.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await HandleException(context, ex, HttpStatusCode.BadRequest);
        }
        catch (ApplicationException ex)
        {
            await HandleException(context, ex, HttpStatusCode.BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleException(context, ex, HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleException(
                context,
                ex,
                HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleException(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = exception.Message,
            status = context.Response.StatusCode,
            code = GetINextCode(exception.Message),
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
    
    private static string GetINextCode(string message) => message.ToLower().Replace(' ', '_');
}