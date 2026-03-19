using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Products;

public class ProductImage : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}