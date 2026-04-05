using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class GuideDistrict : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public Guid DistrictId { get; set; }
    public District District { get; set; } = default!;
}
