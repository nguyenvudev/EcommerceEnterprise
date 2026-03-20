using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Enums;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Queries.GetMyOrders;

public record GetMyOrdersQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 10) : IRequest<PagedResult<OrderSummaryDto>>;

public record OrderSummaryDto(
    Guid Id,
    string OrderNumber,
    OrderStatus Status,
    decimal TotalAmount,
    int TotalItems,
    DateTime CreatedAt);