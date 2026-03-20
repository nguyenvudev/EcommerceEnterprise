using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Features.Orders.Commands.CancelOrder;
using EcommerceEnterprise.Application.Features.Orders.Commands.PlaceOrder;
using EcommerceEnterprise.Application.Features.Orders.Queries.GetMyOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OrdersController(
    IMediator mediator,
    ICurrentUserService currentUser) : ControllerBase
{
    /// <summary>Đặt hàng mới</summary>
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] PlaceOrderCommand command,
        CancellationToken ct)
    {
        // Gán UserId từ token vào command
        var cmd = command with { UserId = currentUser.UserId!.Value };
        var result = await mediator.Send(cmd, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetMyOrders),
                new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Lấy danh sách đơn hàng của tôi</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetMyOrdersQuery(
                currentUser.UserId!.Value, page, pageSize), ct);

        return Ok(result);
    }

    /// <summary>Hủy đơn hàng</summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(
        Guid id,
        [FromBody] CancelOrderRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new CancelOrderCommand(
                id, currentUser.UserId!.Value, request.Reason), ct);

        return result.IsSuccess
            ? Ok(new { message = "Đã hủy đơn hàng." })
            : BadRequest(new { error = result.Error });
    }
}

public record CancelOrderRequest(string Reason);