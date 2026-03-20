using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    Guid ProductId,
    Guid UserId,
    Guid OrderItemId,
    byte Rating,
    string Content) : IRequest<Result<Guid>>;