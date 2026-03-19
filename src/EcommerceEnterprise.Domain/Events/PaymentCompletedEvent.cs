using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Events;

public record PaymentCompletedEvent(Guid PaymentId, Guid OrderId, decimal Amount) : IDomainEvent;