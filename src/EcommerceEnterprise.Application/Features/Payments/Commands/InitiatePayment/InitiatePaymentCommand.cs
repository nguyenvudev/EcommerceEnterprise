using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Enums;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Payments.Commands.InitiatePayment;

public record InitiatePaymentCommand(
    Guid OrderId,
    PaymentMethod Method) : IRequest<Result<InitiatePaymentResponse>>;

public record InitiatePaymentResponse(
    Guid PaymentId,
    string? PaymentUrl);