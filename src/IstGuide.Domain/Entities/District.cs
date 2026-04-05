using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class District : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int SortOrder { get; set; }
    public bool IsPopular { get; set; }

    public ICollection<GuideDistrict> GuideDistricts { get; set; } = new List<GuideDistrict>();
    public ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
