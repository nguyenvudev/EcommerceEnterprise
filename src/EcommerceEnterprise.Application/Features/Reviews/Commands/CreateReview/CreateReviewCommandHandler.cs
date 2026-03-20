using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Entities.Reviews;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler(
    IRepository<Review> reviewRepo,
    IRepository<OrderItem> orderItemRepo,
    IUnitOfWork uow)
    : IRequestHandler<CreateReviewCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateReviewCommand request, CancellationToken ct)
    {
        // 1. Kiểm tra đã review chưa
        var existing = await reviewRepo.FindAsync(
            r => r.UserId == request.UserId &&
                 r.OrderItemId == request.OrderItemId, ct);

        if (existing.Any())
            return Result<Guid>.Failure("Bạn đã đánh giá sản phẩm này rồi.");

        // 2. Kiểm tra đã mua hàng chưa
        var orderItem = await orderItemRepo.GetByIdAsync(request.OrderItemId, ct);
        if (orderItem == null)
            return Result<Guid>.Failure("Không tìm thấy sản phẩm trong đơn hàng.", 404);

        // 3. Tạo review
        var review = Review.Create(
            request.ProductId,
            request.UserId,
            request.OrderItemId,
            request.Rating,
            request.Content);

        // 4. Đánh dấu đã review
        orderItem.IsReviewed = true;
        orderItemRepo.Update(orderItem);

        await reviewRepo.AddAsync(review, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(review.Id, 201);
    }
}