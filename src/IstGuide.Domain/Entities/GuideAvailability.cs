using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class GuideAvailability : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
}
