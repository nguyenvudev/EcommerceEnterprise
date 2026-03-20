using EcommerceEnterprise.Application.Features.Payments.Commands.InitiatePayment;
using EcommerceEnterprise.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>Khởi tạo thanh toán VNPay</summary>
    [HttpPost("vnpay/create")]
    public async Task<IActionResult> CreateVNPayPayment(
        [FromBody] CreatePaymentRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new InitiatePaymentCommand(
                request.OrderId, PaymentMethod.VNPay), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Khởi tạo thanh toán MoMo</summary>
    [HttpPost("momo/create")]
    public async Task<IActionResult> CreateMoMoPayment(
        [FromBody] CreatePaymentRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new InitiatePaymentCommand(
                request.OrderId, PaymentMethod.MoMo), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }
}

public record CreatePaymentRequest(Guid OrderId);