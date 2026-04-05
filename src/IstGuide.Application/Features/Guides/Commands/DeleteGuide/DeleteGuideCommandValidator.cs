using FluentValidation;

namespace IstGuide.Application.Features.Guides.Commands.DeleteGuide;

public class DeleteGuideCommandValidator : AbstractValidator<DeleteGuideCommand>
{
    public DeleteGuideCommandValidator()
    {
        RuleFor(x => x.GuideId)
            .NotEmpty().WithMessage("Guide ID is required.");
    }
}
