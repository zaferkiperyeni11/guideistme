using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class GuideSpecialty : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = default!;
}
