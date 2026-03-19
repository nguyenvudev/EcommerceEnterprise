namespace EcommerceEnterprise.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(
        string toEmail, string orderNumber,
        decimal total, CancellationToken ct = default);

    Task SendPasswordResetAsync(
        string toEmail, string resetLink,
        CancellationToken ct = default);

    Task SendRefundNotificationAsync(
        string toEmail, string orderNumber,
        decimal refundAmount, CancellationToken ct = default);
}