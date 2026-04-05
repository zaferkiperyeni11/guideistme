using FluentValidation;

namespace IstGuide.Application.Features.Tours.Commands.DeleteTour;

public class DeleteTourCommandValidator : AbstractValidator<DeleteTourCommand>
{
    public DeleteTourCommandValidator()
    {
        RuleFor(x => x.TourId)
            .NotEmpty().WithMessage("Tur ID zorunludur.");
    }
}
