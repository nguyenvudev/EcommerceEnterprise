using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Events;

public record OrderConfirmedEvent(Guid OrderId, Guid UserId) : IDomainEvent;