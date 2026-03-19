namespace EcommerceEnterprise.Application.Common.Interfaces;

public interface IPaymentGateway
{
    Task<string> CreatePaymentUrlAsync(
        Guid orderId, decimal amount,
        string orderInfo, CancellationToken ct = default);

    Task<PaymentCallbackResult> VerifyCallbackAsync(
        IDictionary<string, string> parameters);
}

public record PaymentCallbackResult(
    bool IsSuccess,
    string TransactionId,
    string RawResponse);