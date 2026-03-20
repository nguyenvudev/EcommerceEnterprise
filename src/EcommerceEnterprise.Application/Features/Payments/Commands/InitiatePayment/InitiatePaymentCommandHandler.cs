using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Entities.Payments;
using EcommerceEnterprise.Domain.Enums;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Payments.Commands.InitiatePayment;

public class InitiatePaymentCommandHandler(
    IRepository<Order> orderRepo,
    IRepository<Payment> paymentRepo,
    IPaymentGateway paymentGateway,
    IUnitOfWork uow)
    : IRequestHandler<InitiatePaymentCommand, Result<InitiatePaymentResponse>>
{
    public async Task<Result<InitiatePaymentResponse>> Handle(
        InitiatePaymentCommand request, CancellationToken ct)
    {
        // 1. Tìm đơn hàng
        var order = await orderRepo.GetByIdAsync(request.OrderId, ct);
        if (order == null)
            return Result<InitiatePaymentResponse>.Failure(
                "Không tìm thấy đơn hàng.", 404);

        // 2. Tạo Payment record
        var payment = new Payment
        {
            OrderId = order.Id,
            Method = request.Method,
            Amount = order.TotalAmount,
            TxnRef = $"{order.Id.ToString()[..8]}{DateTime.UtcNow:HHmmss}"
        };

        await paymentRepo.AddAsync(payment, ct);
        await uow.SaveChangesAsync(ct);

        // 3. Tạo URL thanh toán (chỉ cho VNPay và MoMo)
        string? paymentUrl = null;

        if (request.Method == PaymentMethod.VNPay ||
            request.Method == PaymentMethod.MoMo)
        {
            paymentUrl = await paymentGateway.CreatePaymentUrlAsync(
                order.Id,
                order.TotalAmount,
                $"Thanh toan don hang {order.OrderNumber}",
                ct);
        }

        return Result<InitiatePaymentResponse>.Success(
            new InitiatePaymentResponse(payment.Id, paymentUrl));
    }
}