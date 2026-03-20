using FluentValidation;

namespace EcommerceEnterprise.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được trống.")
            .MaximumLength(500);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Slug chỉ được chứa chữ thường, số và dấu gạch ngang.");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Danh mục không được trống.");
    }
}