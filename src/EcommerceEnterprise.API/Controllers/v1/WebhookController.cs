using EcommerceEnterprise.Application.Features.Payments.Commands.HandleVNPayCallback;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers;

[ApiController]
[Route("api/v1/webhook")]
public class WebhookController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// VNPay callback — gọi sau khi khách thanh toán xong
    /// KHÔNG đặt [Authorize] vì VNPay gọi từ server của họ
    /// </summary>
    [HttpGet("vnpay-return")]
    public async Task<IActionResult> VNPayReturn(
        CancellationToken ct)
    {
        // Lấy tất cả query params từ VNPay
        var parameters = Request.Query
            .ToDictionary(
                q => q.Key,
                q => q.Value.ToString());

        var result = await mediator.Send(
            new HandleVNPayCallbackCommand(parameters), ct);

        if (result.IsSuccess)
        {
            // Redirect về trang thành công trên Angular
            return Redirect(
                "http://localhost:4200/orders?payment=success");
        }

        return Redirect(
            "http://localhost:4200/orders?payment=failed");
    }

    /// <summary>VNPay IPN — server-to-server notification</summary>
    [HttpPost("vnpay-ipn")]
    public async Task<IActionResult> VNPayIPN(
        CancellationToken ct)
    {
        var parameters = Request.Query
            .ToDictionary(
                q => q.Key,
                q => q.Value.ToString());

        var result = await mediator.Send(
            new HandleVNPayCallbackCommand(parameters), ct);

        // VNPay yêu cầu trả về format cụ thể
        return result.IsSuccess
            ? Ok(new { RspCode = "00", Message = "Confirm Success" })
            : Ok(new { RspCode = "99", Message = "Confirm Fail" });
    }
}