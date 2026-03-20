using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Payments.Commands.HandleVNPayCallback;

public record HandleVNPayCallbackCommand(
    IDictionary<string, string> Parameters) : IRequest<Result>;