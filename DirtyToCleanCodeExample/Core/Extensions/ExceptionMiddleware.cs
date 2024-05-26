using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions;
/// <summary>
/// Middleware for handling exceptions and returning appropriate error responses in JSON format.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware asynchronously.
    /// </summary>
    /// <param name="httpContext">The HttpContext for the current request.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(httpContext, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        // Default error message and status code
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            StatusCode = statusCode,
            Message = exception.Message
        };

        // Set the status code based on the exception type
        switch (exception)
        {
            case ArgumentException _:
                errorDetails.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case UnauthorizedAccessException _:
                errorDetails.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
                // Diğer istisna türleri için case'ler buraya eklenebilir...
        }

        return httpContext.Response.WriteAsync(errorDetails.ToString());
    }
}