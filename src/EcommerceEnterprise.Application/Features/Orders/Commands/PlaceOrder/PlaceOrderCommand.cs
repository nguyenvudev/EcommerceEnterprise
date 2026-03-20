using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Orders.Commands.PlaceOrder;

public record PlaceOrderCommand(
    Guid UserId,
    Guid BranchId,
    Guid ShippingAddressId,
    decimal ShippingFee,
    List<OrderItemRequest> Items,
    string? CouponCode,
    string? Note) : IRequest<Result<Guid>>;

public record OrderItemRequest(
    Guid VariantId,
    string ProductName,
    string VariantAttributes,
    decimal UnitPrice,
    int Quantity);