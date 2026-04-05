using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;
using IstGuide.Domain.Enums;

namespace IstGuide.Application.Features.Guides.Queries.GetGuideBySlug;

public class GuideDetailDto : GuideListDto
{
    public string? DetailedDescription { get; set; }
    public string? LicenseNumber { get; set; }
    public int YearsOfExperience { get; set; }
    public Gender Gender { get; set; }
    public List<GuidePhotoDto> Photos { get; set; } = new();
    public List<GuideCertificateDto> Certificates { get; set; } = new();
    public List<GuideAvailabilityDto> Availabilities { get; set; } = new();
    public List<ReviewDto> RecentReviews { get; set; } = new();
}

public class GuidePhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class GuideCertificateDto
{
    public Guid Id { get; set; }
    public string CertificateName { get; set; } = default!;
    public string? IssuingAuthority { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsVerified { get; set; }
}

public class GuideAvailabilityDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
