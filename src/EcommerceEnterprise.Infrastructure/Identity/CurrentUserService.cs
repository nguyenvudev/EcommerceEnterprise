using System.Security.Claims;
using EcommerceEnterprise.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceEnterprise.Infrastructure.Identity;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User
        => httpContextAccessor.HttpContext?.User;

    public Guid? UserId
        => Guid.TryParse(
            User?.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id) ? id : null;

    public string? Email
        => User?.FindFirstValue(ClaimTypes.Email);

    public string? Role
        => User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated
        => User?.Identity?.IsAuthenticated ?? false;
}