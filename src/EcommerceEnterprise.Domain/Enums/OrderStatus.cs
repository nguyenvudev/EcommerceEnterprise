namespace EcommerceEnterprise.Domain.Enums;

public enum OrderStatus : byte
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipping = 3,
    Delivered = 4,
    Cancelled = 5,
    Refunded = 6
}