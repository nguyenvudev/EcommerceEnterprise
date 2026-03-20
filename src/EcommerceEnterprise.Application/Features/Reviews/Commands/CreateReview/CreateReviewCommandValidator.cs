using FluentValidation;

namespace EcommerceEnterprise.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator
    : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween((byte)1, (byte)5)
            .WithMessage("Rating phải từ 1 đến 5.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung đánh giá không được trống.")
            .MinimumLength(10).WithMessage("Nội dung tối thiểu 10 ký tự.")
            .MaximumLength(2000);
    }
}