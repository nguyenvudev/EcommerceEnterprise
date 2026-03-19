using EcommerceEnterprise.Domain.Entities.Users;

namespace EcommerceEnterprise.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}