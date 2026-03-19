using EcommerceEnterprise.Application.Common.Models;
using MediatR;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.RefreshTokenCommand;

public record RefreshTokenCommand(
    string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;

public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken);