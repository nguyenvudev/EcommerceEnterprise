using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Wallets.Commands.CreditWallet;

public class CreditWalletCommandHandler(
    IRepository<Wallet> walletRepo,
    IUnitOfWork uow)
    : IRequestHandler<CreditWalletCommand, Result>
{
    public async Task<Result> Handle(
        CreditWalletCommand request, CancellationToken ct)
    {
        var wallets = await walletRepo.FindAsync(
            w => w.UserId == request.UserId, ct);

        var wallet = wallets.FirstOrDefault();

        if (wallet == null)
            return Result.Failure("Không tìm thấy ví.", 404);

        // Credit sẽ throw nếu amount <= 0
        wallet.Credit(request.Amount);
        walletRepo.Update(wallet);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}