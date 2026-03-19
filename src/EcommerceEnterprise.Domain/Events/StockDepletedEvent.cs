using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Events;

public record StockDepletedEvent(Guid VariantId, Guid WarehouseId) : IDomainEvent;