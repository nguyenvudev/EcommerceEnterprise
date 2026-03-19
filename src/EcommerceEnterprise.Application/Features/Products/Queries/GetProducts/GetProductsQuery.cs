using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    decimal? MinRating,
    string? SearchTerm,
    string? SortBy,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<ProductListDto>>;

public record ProductListDto(
    Guid Id,
    string Name,
    string Slug,
    decimal BasePrice,
    decimal AverageRating,
    int TotalReviews,
    string? PrimaryImageUrl,
    bool HasFlashSale,
    decimal? FlashSalePrice);