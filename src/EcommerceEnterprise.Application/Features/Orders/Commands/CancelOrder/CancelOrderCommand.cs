using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommand(
    Guid OrderId,
    Guid UserId,
    string Reason) : IRequest<Result>;