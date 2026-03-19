using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Products;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler(
    IRepository<Product> productRepo)
    : IRequestHandler<GetProductsQuery, PagedResult<ProductListDto>>
{
    public async Task<PagedResult<ProductListDto>> Handle(
        GetProductsQuery request, CancellationToken ct)
    {
        // Lấy tất cả sản phẩm active
        var all = await productRepo.FindAsync(
            p => p.IsActive && !p.IsDeleted, ct);

        // Filter
        if (request.CategoryId.HasValue)
            all = all.Where(p => p.CategoryId == request.CategoryId.Value);

        if (request.MinPrice.HasValue)
            all = all.Where(p => p.BasePrice >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            all = all.Where(p => p.BasePrice <= request.MaxPrice.Value);

        if (request.MinRating.HasValue)
            all = all.Where(p => p.AverageRating >= request.MinRating.Value);

        if (!string.IsNullOrEmpty(request.SearchTerm))
            all = all.Where(p =>
                p.Name.Contains(request.SearchTerm,
                    StringComparison.OrdinalIgnoreCase));

        // Sort
        all = request.SortBy switch
        {
            "price_asc" => all.OrderBy(p => p.BasePrice),
            "price_desc" => all.OrderByDescending(p => p.BasePrice),
            "rating" => all.OrderByDescending(p => p.AverageRating),
            "newest" => all.OrderByDescending(p => p.CreatedAt),
            _ => all.OrderByDescending(p => p.CreatedAt)
        };

        // Phân trang
        var totalCount = all.Count();
        var items = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductListDto(
                p.Id,
                p.Name,
                p.Slug,
                p.BasePrice,
                p.AverageRating,
                p.TotalReviews,
                p.Images.FirstOrDefault(i => i.IsPrimary)?.Url,
                false,
                null))
            .ToList();

        return new PagedResult<ProductListDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}