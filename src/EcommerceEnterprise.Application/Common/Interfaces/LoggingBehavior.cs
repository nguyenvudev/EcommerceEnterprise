using MediatR;
using Microsoft.Extensions.Logging;

namespace EcommerceEnterprise.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var name = typeof(TRequest).Name;

        logger.LogInformation("→ Bắt đầu xử lý {RequestName}", name);

        var response = await next();

        logger.LogInformation("✓ Xong {RequestName}", name);

        return response;
    }
}