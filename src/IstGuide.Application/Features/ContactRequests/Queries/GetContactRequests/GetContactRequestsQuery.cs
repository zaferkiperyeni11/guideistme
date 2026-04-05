using IstGuide.Domain.Enums;
using MediatR;

namespace IstGuide.Application.Features.ContactRequests.Queries.GetContactRequests;

public record GetContactRequestsQuery : IRequest<IReadOnlyList<ContactRequestDto>>
{
    public ContactRequestStatus? Status { get; init; }
    public Guid? GuideId { get; init; }
}

public class ContactRequestDto
{
    public Guid Id { get; set; }
    public Guid GuideId { get; set; }
    public string GuideName { get; set; } = default!;
    public string VisitorName { get; set; } = default!;
    public string VisitorEmail { get; set; } = default!;
    public string? VisitorPhone { get; set; }
    public string Message { get; set; } = default!;
    public DateTime? PreferredDate { get; set; }
    public int? GroupSize { get; set; }
    public ContactRequestStatus Status { get; set; }
    public ContactSource Source { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AdminNotes { get; set; }
}
