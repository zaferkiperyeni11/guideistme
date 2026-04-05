using System.Net;
using System.Text.Json;
using IstGuide.Application.Common.Models;

namespace IstGuide.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Hata türüne göre HTTP status kodu ayarlanabilir, şimdilik hepsi için 500
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = Result.Failure(exception.Message);
        var json = JsonSerializer.Serialize(result);

        await context.Response.WriteAsync(json);
    }
}
