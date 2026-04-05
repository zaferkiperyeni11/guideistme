using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Entities;

public class GuideLanguage : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public Guid LanguageId { get; set; }
    public Language Language { get; set; } = default!;
    public LanguageProficiency Proficiency { get; set; }
}
