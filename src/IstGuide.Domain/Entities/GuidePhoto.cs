using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class GuidePhoto : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}
