using EcommerceEnterprise.Application.Common.Interfaces;
using EcommerceEnterprise.Application.Common.Models;
using EcommerceEnterprise.Domain.Entities.Users;
using EcommerceEnterprise.Domain.Interfaces;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IRepository<User> userRepo,
    IRepository<RefreshToken> tokenRepo,
    IJwtTokenService jwtService,
    IUnitOfWork uow)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request, CancellationToken ct)
    {
        // 1. Tìm user theo email
        var users = await userRepo.FindAsync(
            u => u.Email == request.Email.ToLower(), ct);
        var user = users.FirstOrDefault();

        // 2. Kiểm tra password
        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<LoginResponse>.Failure(
                "Email hoặc mật khẩu không đúng.", 401);

        // 3. Kiểm tra tài khoản còn hoạt động
        if (!user.IsActive)
            return Result<LoginResponse>.Failure(
                "Tài khoản đã bị vô hiệu hóa.", 403);

        // 4. Tạo tokens
        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshTokenValue = jwtService.GenerateRefreshToken();

        // 5. Lưu refresh token vào DB
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await tokenRepo.AddAsync(refreshToken, ct);
        await uow.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse(
            accessToken,
            refreshTokenValue,
            user.FullName,
            user.Email,
            user.Role.ToString()));
    }
}