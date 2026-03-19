using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Warehouses;

public class WarehouseStock : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderLevel { get; set; } = 10;
    public int AvailableQuantity => Quantity - ReservedQuantity;

    public void Reserve(int qty)
    {
        if (qty > AvailableQuantity)
            throw new InsufficientStockException(VariantId.ToString(), qty, AvailableQuantity);
        ReservedQuantity += qty;
    }

    public void ConfirmSale(int qty)
    {
        Quantity -= qty;
        ReservedQuantity -= qty;
    }
}