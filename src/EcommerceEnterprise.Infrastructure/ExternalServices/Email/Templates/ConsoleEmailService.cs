using EcommerceEnterprise.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace EcommerceEnterprise.Infrastructure.ExternalServices.Email;

public class ConsoleEmailService(
    ILogger<ConsoleEmailService> logger) : IEmailService
{
    public Task SendOrderConfirmationAsync(
        string toEmail, string orderNumber,
        decimal total, CancellationToken ct = default)
    {
        logger.LogInformation(
            "[EMAIL] Gửi xác nhận đơn hàng {OrderNumber} " +
            "tới {Email} — Tổng: {Total:N0} VND",
            orderNumber, toEmail, total);

        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(
        string toEmail, string resetLink,
        CancellationToken ct = default)
    {
        logger.LogInformation(
            "[EMAIL] Gửi reset password tới {Email} — Link: {Link}",
            toEmail, resetLink);

        return Task.CompletedTask;
    }

    public Task SendRefundNotificationAsync(
        string toEmail, string orderNumber,
        decimal refundAmount, CancellationToken ct = default)
    {
        logger.LogInformation(
            "[EMAIL] Gửi thông báo hoàn tiền {Amount:N0} VND " +
            "đơn {OrderNumber} tới {Email}",
            refundAmount, orderNumber, toEmail);

        return Task.CompletedTask;
    }
}