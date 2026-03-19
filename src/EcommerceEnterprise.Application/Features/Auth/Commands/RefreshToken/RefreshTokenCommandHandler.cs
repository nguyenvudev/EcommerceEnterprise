using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.RefreshTokenCommand;

public class RefreshTokenCommandHandler(
    IRepository<RefreshToken> tokenRepo,
    IRepository<User> userRepo,
    IJwtTokenService jwtService,
    IUnitOfWork uow)
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request, CancellationToken ct)
    {
        // 1. Tìm refresh token trong DB
        var tokens = await tokenRepo.FindAsync(
            t => t.Token == request.RefreshToken, ct);
        var token = tokens.FirstOrDefault();

        // 2. Kiểm tra hợp lệ
        if (token == null || !token.IsActive)
            return Result<RefreshTokenResponse>.Failure(
                "Refresh token không hợp lệ hoặc đã hết hạn.", 401);

        // 3. Lấy user
        var user = await userRepo.GetByIdAsync(token.UserId, ct);
        if (user == null || !user.IsActive)
            return Result<RefreshTokenResponse>.Failure(
                "Tài khoản không tồn tại.", 401);

        // 4. Thu hồi token cũ
        token.IsRevoked = true;
        tokenRepo.Update(token);

        // 5. Tạo token mới
        var newAccessToken = jwtService.GenerateAccessToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        await tokenRepo.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, ct);

        await uow.SaveChangesAsync(ct);

        return Result<RefreshTokenResponse>.Success(
            new RefreshTokenResponse(newAccessToken, newRefreshToken));
    }
}