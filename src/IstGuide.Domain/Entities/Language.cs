using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string NativeName { get; set; } = default!;
    public string? FlagIconUrl { get; set; }

    public ICollection<GuideLanguage> GuideLanguages { get; set; } = new List<GuideLanguage>();
}
