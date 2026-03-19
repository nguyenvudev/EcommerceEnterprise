using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Enums;

namespace EcommerceEnterprise.Domain.Entities.Warehouses;

public class StockMovement : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public Guid VariantId { get; set; }
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public int BalanceAfter { get; set; }
    public string? Reference { get; set; }
    public string? Note { get; set; }
    public Guid CreatedByUserId { get; set; }
}