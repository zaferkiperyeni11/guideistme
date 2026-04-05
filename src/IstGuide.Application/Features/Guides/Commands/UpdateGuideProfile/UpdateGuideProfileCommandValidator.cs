using FluentValidation;

namespace IstGuide.Application.Features.Guides.Commands.UpdateGuideProfile;

public class UpdateGuideProfileCommandValidator : AbstractValidator<UpdateGuideProfileCommand>
{
    public UpdateGuideProfileCommandValidator()
    {
        RuleFor(x => x.GuideId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Bio).NotEmpty().MaximumLength(500);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).LessThanOrEqualTo(60);
        RuleFor(x => x.LanguageIds).NotEmpty().WithMessage("En az bir dil seçmelisiniz.");
        RuleFor(x => x.SpecialtyIds).NotEmpty().WithMessage("En az bir uzmanlık alanı seçmelisiniz.");
        RuleFor(x => x.DistrictIds).NotEmpty().WithMessage("En az bir bölge seçmelisiniz.");
    }
}
