using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    string FullName,
    string Email,
    string Role);