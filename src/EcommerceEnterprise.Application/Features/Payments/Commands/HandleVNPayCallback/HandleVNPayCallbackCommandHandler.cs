using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Entities.Payments;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Payments.Commands.HandleVNPayCallback;

public class HandleVNPayCallbackCommandHandler(
    IRepository<Payment> paymentRepo,
    IRepository<Order> orderRepo,
    IPaymentGateway paymentGateway,
    IUnitOfWork uow)
    : IRequestHandler<HandleVNPayCallbackCommand, Result>
{
    public async Task<Result> Handle(
        HandleVNPayCallbackCommand request, CancellationToken ct)
    {
        // 1. Verify chữ ký từ VNPay
        var callbackResult = await paymentGateway
            .VerifyCallbackAsync(request.Parameters);

        if (!callbackResult.IsSuccess)
            return Result.Failure("Chữ ký thanh toán không hợp lệ.", 400);

        // 2. Tìm payment theo TxnRef
        var txnRef = request.Parameters.TryGetValue("vnp_TxnRef", out var value)
            ? value
            : ""; var payments = await paymentRepo.FindAsync(
                    p => p.TxnRef == txnRef, ct);

        var payment = payments.FirstOrDefault();
        if (payment == null)
            return Result.Failure("Không tìm thấy giao dịch.", 404);

        // 3. Cập nhật Payment
        payment.MarkAsPaid(
            callbackResult.TransactionId,
            callbackResult.RawResponse);
        paymentRepo.Update(payment);

        // 4. Confirm Order
        var order = await orderRepo.GetByIdAsync(payment.OrderId, ct);
        if (order != null)
        {
            order.Confirm();
            orderRepo.Update(order);
        }

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}