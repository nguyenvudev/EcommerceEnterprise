using FluentValidation;

namespace EcommerceEnterprise.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ hoa.")
            .Matches("[0-9]").WithMessage("Mật khẩu phải có ít nhất 1 chữ số.");

        RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Họ tên không được trống.")
          .MaximumLength(200);
    }
}