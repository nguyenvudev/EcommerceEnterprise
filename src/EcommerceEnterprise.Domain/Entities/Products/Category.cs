using EcommerceEnterprise.Domain.Common;

namespace EcommerceEnterprise.Domain.Entities.Products;

public class Category : BaseEntity
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}