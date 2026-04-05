using FluentValidation;

namespace IstGuide.Application.Features.Guides.Commands.RegisterGuide;

public class RegisterGuideCommandValidator : AbstractValidator<RegisterGuideCommand>
{
    public RegisterGuideCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Geçerli bir telefon numarası giriniz (10-15 rakam, + ile başlayabilir).");
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Bio).NotEmpty().MaximumLength(500);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).LessThanOrEqualTo(60);
        RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.Today.AddYears(-18))
            .WithMessage("Rehber en az 18 yaşında olmalıdır.");
        RuleFor(x => x.LanguageIds).NotEmpty().WithMessage("En az bir dil seçmelisiniz.");
        RuleFor(x => x.SpecialtyIds).NotEmpty().WithMessage("En az bir uzmanlık alanı seçmelisiniz.");
        RuleFor(x => x.DistrictIds).NotEmpty().WithMessage("En az bir bölge seçmelisiniz.");
    }
}
