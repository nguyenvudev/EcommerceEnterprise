using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Events;

public record OrderPlacedEvent(Guid OrderId, Guid UserId, decimal TotalAmount) : IDomainEvent;