using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class Tour : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Duration { get; set; } = string.Empty; // e.g. "4 Hours", "Full Day"
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }

    // İlişkiler
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = null!;

    public Guid? DistrictId { get; set; }
    public District? District { get; set; }
}
