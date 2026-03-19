using EcommerceEnterprise.Domain.Common;
using EcommerceEnterprise.Domain.Exceptions;

namespace EcommerceEnterprise.Domain.Entities.Products;

public class Product : AggregateRoot
{
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal BasePrice { get; private set; }
    public decimal AverageRating { get; private set; }
    public int TotalReviews { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<ProductVariant> _variants = new();
    public IReadOnlyList<ProductVariant> Variants => _variants.AsReadOnly();

    private readonly List<ProductImage> _images = new();
    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    protected Product() { }

    public static Product Create(Guid categoryId, string name, string slug,
        string description, decimal basePrice)
    {
        if (basePrice < 0)
            throw new DomainException("Giá sản phẩm không thể âm.");

        return new Product
        {
            CategoryId = categoryId,
            Name = name,
            Slug = slug,
            Description = description,
            BasePrice = basePrice
        };
    }

    public ProductVariant AddVariant(string sku, string attributesJson,
        decimal price, decimal weight)
    {
        if (_variants.Any(v => v.SKU == sku))
            throw new DomainException($"SKU '{sku}' đã tồn tại.");

        var variant = ProductVariant.Create(Id, sku, attributesJson, price, weight);
        _variants.Add(variant);
        return variant;
    }

    public void UpdateRating(decimal newAverage, int totalReviews)
    {
        AverageRating = Math.Round(newAverage, 2);
        TotalReviews = totalReviews;
    }
}