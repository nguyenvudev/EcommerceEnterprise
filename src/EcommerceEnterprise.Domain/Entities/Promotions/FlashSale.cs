using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Promotions;

public class FlashSale : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public byte Status { get; set; }
    public string? BannerUrl { get; set; }
    public bool IsActive => DateTime.UtcNow >= StartTime && DateTime.UtcNow <= EndTime;
}