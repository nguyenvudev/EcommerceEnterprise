using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IRepository<User> userRepo,
    IRepository<Wallet> walletRepo,
    IUnitOfWork uow)
    : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        RegisterCommand request, CancellationToken ct)
    {
        // 1. Kiểm tra email đã tồn tại chưa
        var existing = await userRepo.FindAsync(
            u => u.Email == request.Email.ToLower(), ct);

        if (existing.Any())
            return Result<Guid>.Failure("Email đã được đăng ký.");

        // 2. Hash password
        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Tạo User
        var user = User.Create(
            request.Email,
            hash,
            request.FullName,
            request.PhoneNumber);

        // 4. Tạo Wallet đi kèm
        var wallet = Wallet.Create(user.Id);

        // 5. Lưu cả hai cùng lúc
        await userRepo.AddAsync(user, ct);
        await walletRepo.AddAsync(wallet, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(user.Id, 201);
    }
}