using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Products.Queries.GetProductDetail;

public record GetProductDetailQuery(string Slug)
    : IRequest<Result<ProductDetailDto>>;

public record ProductDetailDto(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    decimal BasePrice,
    decimal AverageRating,
    int TotalReviews,
    Guid CategoryId,
    List<VariantDto> Variants,
    List<string> Images);

public record VariantDto(
    Guid Id,
    string SKU,
    string AttributesJson,
    decimal Price,
    decimal CompareAtPrice,
    bool IsActive);