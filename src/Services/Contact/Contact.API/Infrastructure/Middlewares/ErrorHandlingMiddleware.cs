using Contact.API.Infrastructure.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Contact.API.Infrastructure.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exeption)
        {
            await HandleExceptions(context, exeption);
        }
    }

    private static Task HandleExceptions(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            //TODO: log
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Server Error" }));

        }

        var result = JsonSerializer.Serialize(new { error = exception.Message });

        return context.Response.WriteAsync(result);
    }
}
