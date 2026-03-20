using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Wallets.Commands.CreditWallet;

public record CreditWalletCommand(
    Guid UserId,
    decimal Amount,
    string Description,
    string Reference) : IRequest<Result>;