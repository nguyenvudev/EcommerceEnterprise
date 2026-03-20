using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Features.Reviews.Commands.CreateReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ReviewsController(
    IMediator mediator,
    ICurrentUserService currentUser) : ControllerBase
{
    /// <summary>Tạo đánh giá sản phẩm</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new CreateReviewCommand(
                request.ProductId,
                currentUser.UserId!.Value,
                request.OrderItemId,
                request.Rating,
                request.Content), ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateReview),
                new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }
}

public record CreateReviewRequest(
    Guid ProductId,
    Guid OrderItemId,
    byte Rating,
    string Content);