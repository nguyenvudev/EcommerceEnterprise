namespace EcommerceEnterprise.Domain.Enums;

public enum StockMovementType : byte
{
    In = 0,
    Out = 1,
    Transfer = 2,
    Adjust = 3,
    SaleReserve = 4,
    SaleConfirm = 5
}