using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Entities;

public class ContactRequest : BaseAuditableEntity, IAggregateRoot
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public string VisitorName { get; set; } = default!;
    public string VisitorEmail { get; set; } = default!;
    public string? VisitorPhone { get; set; }
    public string Message { get; set; } = default!;
    public DateTime? PreferredDate { get; set; }
    public int? GroupSize { get; set; }
    public ContactRequestStatus Status { get; set; } = ContactRequestStatus.New;
    public string? AdminNotes { get; set; }
    public ContactSource Source { get; set; } = ContactSource.Website;
}
