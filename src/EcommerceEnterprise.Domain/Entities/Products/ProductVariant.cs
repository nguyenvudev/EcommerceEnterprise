using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Products;

public class ProductVariant : BaseEntity
{
    public Guid ProductId { get; private set; }
    public string SKU { get; private set; } = string.Empty;
    public string AttributesJson { get; private set; } = "{}";
    public decimal Price { get; private set; }
    public decimal CompareAtPrice { get; private set; }
    public decimal Weight { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected ProductVariant() { }

    public static ProductVariant Create(Guid productId, string sku,
        string attributesJson, decimal price, decimal weight)
        => new()
        {
            ProductId = productId,
            SKU = sku,
            AttributesJson = attributesJson,
            Price = price,
            Weight = weight
        };

    public void UpdatePrice(decimal price, decimal compareAtPrice)
    {
        Price = price;
        CompareAtPrice = compareAtPrice;
    }
}