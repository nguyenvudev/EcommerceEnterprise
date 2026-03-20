using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Products;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IRepository<Product> productRepo,
    IUnitOfWork uow)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateProductCommand request, CancellationToken ct)
    {
        // Kiểm tra slug trùng
        var existing = await productRepo.FindAsync(
            p => p.Slug == request.Slug, ct);

        if (existing.Any())
            return Result<Guid>.Failure("Slug đã tồn tại.");

        // Tạo product từ Domain
        var product = Product.Create(
            request.CategoryId,
            request.Name,
            request.Slug,
            request.Description,
            request.BasePrice);

        await productRepo.AddAsync(product, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(product.Id, 201);
    }
}