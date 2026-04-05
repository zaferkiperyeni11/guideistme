using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class PageContent : BaseAuditableEntity
{
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsPublished { get; set; }
}
