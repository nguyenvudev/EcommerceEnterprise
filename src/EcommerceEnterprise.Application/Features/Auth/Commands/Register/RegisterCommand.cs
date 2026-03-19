using EcommerceEnterprise.Application.Common.Models;
using MediatR;
namespace EcommerceEnterprise.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber) : IRequest<Result<Guid>>;