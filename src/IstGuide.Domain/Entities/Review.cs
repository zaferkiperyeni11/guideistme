using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Entities;

public class Review : BaseAuditableEntity, IAggregateRoot
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public string ReviewerName { get; set; } = default!;
    public string? ReviewerEmail { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? AdminReply { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public string? ReviewerCountry { get; set; }
    public string? ReviewerLanguage { get; set; }
}
