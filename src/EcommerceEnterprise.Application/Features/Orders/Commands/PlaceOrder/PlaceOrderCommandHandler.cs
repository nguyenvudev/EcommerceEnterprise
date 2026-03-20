using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Entities.Promotions;
using EcommerceEnterprise.Domain.Entities.Warehouses;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(
    IRepository<Order> orderRepo,
    IRepository<WarehouseStock> stockRepo,
    IRepository<Coupon> couponRepo,
    IUnitOfWork uow)
    : IRequestHandler<PlaceOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        PlaceOrderCommand request, CancellationToken ct)
    {
        // 1. Tạo đơn hàng
        var order = Order.Create(
            request.UserId,
            request.BranchId,
            request.ShippingAddressId,
            request.ShippingFee,
            request.Note);

        // 2. Thêm từng item + kiểm tra stock
        foreach (var item in request.Items)
        {
            var stocks = await stockRepo.FindAsync(
                s => s.VariantId == item.VariantId, ct);

            var stock = stocks.FirstOrDefault();

            if (stock == null)
                return Result<Guid>.Failure(
                    $"Không tìm thấy tồn kho cho sản phẩm.");

            // Reserve stock — sẽ throw nếu không đủ hàng
            stock.Reserve(item.Quantity);
            stockRepo.Update(stock);

            order.AddItem(
                item.VariantId,
                item.ProductName,
                item.VariantAttributes,
                item.UnitPrice,
                item.Quantity);
        }

        // 3. Áp dụng coupon nếu có
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var coupons = await couponRepo.FindAsync(
                c => c.Code == request.CouponCode.ToUpper(), ct);

            var coupon = coupons.FirstOrDefault();

            if (coupon == null)
                return Result<Guid>.Failure("Coupon không tồn tại.");

            // CalculateDiscount sẽ throw nếu coupon không hợp lệ
            var discount = coupon.CalculateDiscount(order.SubTotal);
            order.ApplyDiscount(coupon.Id, discount);

            coupon.UsedCount++;
            couponRepo.Update(coupon);
        }

        // 4. Lưu tất cả
        await orderRepo.AddAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(order.Id, 201);
    }
}