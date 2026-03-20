using System.Net;
using System.Text.Json;
using EcommerceEnterprise.Domain.Exceptions;
using FluentValidation;

namespace EcommerceEnterprise.API.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            // FluentValidation errors → 400
            logger.LogWarning("Validation failed: {Errors}",
                string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new { type = "ValidationError", errors }));
        }
        catch (DomainException ex)
        {
            // Business rule violation → 400
            logger.LogWarning("Domain exception: {Message}", ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new { type = "DomainError", error = ex.Message }));
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new { type = "Unauthorized", error = ex.Message }));
        }
        catch (Exception ex)
        {
            // Unexpected error → 500
            logger.LogError(ex, "Unhandled exception occurred");

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new
                {
                    type = "ServerError",
                    error = "Đã xảy ra lỗi, vui lòng thử lại sau."
                }));
        }
    }
}