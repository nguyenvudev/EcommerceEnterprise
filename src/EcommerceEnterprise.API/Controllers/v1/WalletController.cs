using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Features.Wallets.Commands.CreditWallet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class WalletController(
    IMediator mediator,
    ICurrentUserService currentUser) : ControllerBase
{
    /// <summary>Nạp tiền vào ví — Admin dùng khi xử lý refund</summary>
    [HttpPost("credit")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Credit(
        [FromBody] CreditWalletRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new CreditWalletCommand(
                request.UserId,
                request.Amount,
                request.Description,
                request.Reference), ct);

        return result.IsSuccess
            ? Ok(new { message = "Nạp tiền thành công." })
            : BadRequest(new { error = result.Error });
    }
}

public record CreditWalletRequest(
    Guid UserId,
    decimal Amount,
    string Description,
    string Reference);