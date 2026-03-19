using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Enums;

namespace EcommerceEnterprise.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public decimal Amount { get; set; }
    public string TxnRef { get; set; } = string.Empty;
    public string? GatewayTransactionId { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime? PaidAt { get; set; }

    public void MarkAsPaid(string gatewayTxnId, string response)
    {
        Status = PaymentStatus.Paid;
        GatewayTransactionId = gatewayTxnId;
        GatewayResponse = response;
        PaidAt = DateTime.UtcNow;
    }
}