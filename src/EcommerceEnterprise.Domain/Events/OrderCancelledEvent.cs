using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Events;

public record OrderCancelledEvent(Guid OrderId, Guid UserId, string Reason) : IDomainEvent;