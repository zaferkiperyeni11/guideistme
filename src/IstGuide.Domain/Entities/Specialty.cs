using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class Specialty : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<GuideSpecialty> GuideSpecialties { get; set; } = new List<GuideSpecialty>();
}
