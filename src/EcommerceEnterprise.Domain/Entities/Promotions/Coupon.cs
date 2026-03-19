using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Promotions;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public bool IsPercentage { get; set; }
    public decimal Value { get; set; }
    public decimal MinOrderAmount { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public int MaxUsageCount { get; set; }
    public int UsedCount { get; set; }
    public int MaxUsagePerUser { get; set; } = 1;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public decimal CalculateDiscount(decimal orderTotal)
    {
        if (!IsActive || DateTime.UtcNow < StartDate || DateTime.UtcNow > EndDate)
            throw new DomainException("Coupon không hợp lệ hoặc đã hết hạn.");
        if (orderTotal < MinOrderAmount)
            throw new DomainException($"Đơn hàng tối thiểu {MinOrderAmount:N0} VND.");
        if (UsedCount >= MaxUsageCount)
            throw new DomainException("Coupon đã hết lượt sử dụng.");

        var discount = IsPercentage ? orderTotal * Value / 100 : Value;
        return Math.Min(discount, MaxDiscountAmount > 0 ? MaxDiscountAmount : discount);
    }
}