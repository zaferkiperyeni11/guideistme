using FluentValidation;

namespace IstGuide.Application.Features.Tours.Commands.CreateTour;

public class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
{
    public CreateTourCommandValidator()
    {
        RuleFor(x => x.GuideId)
            .NotEmpty().WithMessage("Rehber ID zorunludur.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tur başlığı zorunludur.")
            .MaximumLength(200).WithMessage("Tur başlığı en fazla 200 karakter olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Tur açıklaması zorunludur.")
            .MaximumLength(2000).WithMessage("Tur açıklaması en fazla 2000 karakter olmalıdır.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

        RuleFor(x => x.Duration)
            .NotEmpty().WithMessage("Süre zorunludur.")
            .MaximumLength(100).WithMessage("Süre en fazla 100 karakter olmalıdır.");
    }
}
