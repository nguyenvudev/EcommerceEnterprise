using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Promotions;

public class FlashSaleItem : BaseEntity
{
    public Guid FlashSaleId { get; set; }
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public decimal SalePrice { get; set; }
    public decimal OriginalPrice { get; set; }
    public int StockLimit { get; set; }
    public int SoldCount { get; set; }
    public int PerUserLimit { get; set; } = 1;
    public bool IsSoldOut => SoldCount >= StockLimit;
}