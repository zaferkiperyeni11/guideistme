using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;

namespace IstGuide.Application.Features.ContactRequests.Commands.CreateContactRequest;

public record CreateContactRequestCommand : IRequest<Result<Guid>>
{
    public Guid GuideId { get; init; }
    public string VisitorName { get; init; } = default!;
    public string VisitorEmail { get; init; } = default!;
    public string? VisitorPhone { get; init; }
    public string Message { get; init; } = default!;
    public DateTime? PreferredDate { get; init; }
    public int? GroupSize { get; init; }
    public ContactSource Source { get; init; } = ContactSource.Website;
}
