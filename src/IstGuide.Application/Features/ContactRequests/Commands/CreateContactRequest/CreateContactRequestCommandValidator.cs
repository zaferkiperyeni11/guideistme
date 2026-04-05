using FluentValidation;

namespace IstGuide.Application.Features.ContactRequests.Commands.CreateContactRequest;

public class CreateContactRequestCommandValidator : AbstractValidator<CreateContactRequestCommand>
{
    public CreateContactRequestCommandValidator()
    {
        RuleFor(x => x.GuideId).NotEmpty();
        RuleFor(x => x.VisitorName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.VisitorEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.GroupSize).GreaterThan(0).When(x => x.GroupSize.HasValue);
        RuleFor(x => x.PreferredDate).GreaterThan(DateTime.Today).When(x => x.PreferredDate.HasValue);
    }
}
