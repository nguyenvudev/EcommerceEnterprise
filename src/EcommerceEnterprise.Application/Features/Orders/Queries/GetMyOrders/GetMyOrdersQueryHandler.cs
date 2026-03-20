using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Orders;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Queries.GetMyOrders;

public class GetMyOrdersQueryHandler(
    IRepository<Order> orderRepo)
    : IRequestHandler<GetMyOrdersQuery, PagedResult<OrderSummaryDto>>
{
    public async Task<PagedResult<OrderSummaryDto>> Handle(
        GetMyOrdersQuery request, CancellationToken ct)
    {
        var all = await orderRepo.FindAsync(
            o => o.UserId == request.UserId, ct);

        var ordered = all.OrderByDescending(o => o.CreatedAt);
        var totalCount = ordered.Count();

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.OrderNumber,
                o.Status,
                o.TotalAmount,
                o.Items.Count,
                o.CreatedAt))
            .ToList();

        return new PagedResult<OrderSummaryDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}