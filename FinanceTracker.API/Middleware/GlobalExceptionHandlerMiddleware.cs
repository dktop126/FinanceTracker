using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FinanceTracker.API.Middleware;

/// <summary>
/// Middleware для глобальной обработки исключений в приложении.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Произошла необработанная ошибка: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Message = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Ошибка валидации";
                errorResponse.Errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = exception.Message;
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "Доступ запрещен";
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = exception.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Внутренняя ошибка сервера";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}

/// <summary>
/// Модель ответа об ошибке.
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
