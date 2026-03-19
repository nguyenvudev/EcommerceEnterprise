using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Enums;
using EcommerceEnterprise.Domain.Events;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Orders;

public class Order : AggregateRoot
{
    public string OrderNumber { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid ShippingAddressId { get; private set; }
    public Guid? CouponId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal SubTotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Note { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    protected Order() { }

    public static Order Create(Guid userId, Guid branchId,
        Guid shippingAddressId, decimal shippingFee, string? note = null)
    {
        return new Order
        {
            OrderNumber = $"ORD{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString()[..4].ToUpper()}",
            UserId = userId,
            BranchId = branchId,
            ShippingAddressId = shippingAddressId,
            ShippingFee = shippingFee,
            Note = note
        };
    }

    public void AddItem(Guid variantId, string productName,
        string variantAttr, decimal unitPrice, int quantity)
    {
        _items.Add(new OrderItem
        {
            OrderId = Id,
            VariantId = variantId,
            ProductName = productName,
            VariantAttributes = variantAttr,
            UnitPrice = unitPrice,
            Quantity = quantity,
            TotalPrice = unitPrice * quantity
        });
        RecalculateTotals();
    }

    public void ApplyDiscount(Guid couponId, decimal discountAmount)
    {
        CouponId = couponId;
        DiscountAmount = discountAmount;
        RecalculateTotals();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStateException("Chỉ đơn hàng đang chờ mới có thể xác nhận.");
        Status = OrderStatus.Confirmed;
        RaiseDomainEvent(new OrderConfirmedEvent(Id, UserId));
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Shipping or OrderStatus.Delivered)
            throw new InvalidOrderStateException("Không thể hủy đơn đang giao hoặc đã giao.");
        Status = OrderStatus.Cancelled;
        RaiseDomainEvent(new OrderCancelledEvent(Id, UserId, reason));
    }

    private void RecalculateTotals()
    {
        SubTotal = _items.Sum(i => i.TotalPrice);
        TotalAmount = SubTotal - DiscountAmount + ShippingFee;
    }
}