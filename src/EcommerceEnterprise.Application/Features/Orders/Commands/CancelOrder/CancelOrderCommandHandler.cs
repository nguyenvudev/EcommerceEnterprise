using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IRepository<Order> orderRepo,
    IUnitOfWork uow)
    : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(
        CancelOrderCommand request, CancellationToken ct)
    {
        var order = await orderRepo.GetByIdAsync(request.OrderId, ct);

        if (order == null)
            return Result.Failure("Không tìm thấy đơn hàng.", 404);

        // Kiểm tra quyền — chỉ chủ đơn mới được hủy
        if (order.UserId != request.UserId)
            return Result.Failure("Bạn không có quyền hủy đơn này.", 403);

        // Cancel sẽ throw InvalidOrderStateException nếu không hợp lệ
        order.Cancel(request.Reason);

        orderRepo.Update(order);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}