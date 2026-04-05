using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class SiteSettings : BaseEntity
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
    public string GroupName { get; set; } = "General";
}
