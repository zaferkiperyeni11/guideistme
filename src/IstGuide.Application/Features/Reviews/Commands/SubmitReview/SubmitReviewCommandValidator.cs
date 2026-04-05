using FluentValidation;

namespace IstGuide.Application.Features.Reviews.Commands.SubmitReview;

public class SubmitReviewCommandValidator : AbstractValidator<SubmitReviewCommand>
{
    public SubmitReviewCommandValidator()
    {
        RuleFor(x => x.GuideId).NotEmpty();
        RuleFor(x => x.ReviewerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ReviewerEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.ReviewerEmail));
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Puan 1 ile 5 arasında olmalıdır.");
        RuleFor(x => x.Comment).MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Comment));
    }
}
