using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Entities;

public class Guide : BaseAuditableEntity, IAggregateRoot
{
    // Kişisel Bilgiler
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? WhatsAppUrl { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string? CoverPhotoUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    // Profesyonel Bilgiler
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Bio { get; set; } = default!;
    public string? DetailedDescription { get; set; }
    public string? LicenseNumber { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? DailyRate { get; set; }

    // Durum
    public GuideStatus Status { get; set; } = GuideStatus.Pending;
    public bool IsFeatured { get; set; }
    public int ProfileViewCount { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }

    // Identity bağlantısı
    public string? UserId { get; set; }

    // İlişkiler
    public ICollection<GuideLanguage> Languages { get; set; } = new List<GuideLanguage>();
    public ICollection<GuideSpecialty> Specialties { get; set; } = new List<GuideSpecialty>();
    public ICollection<GuideDistrict> ServiceDistricts { get; set; } = new List<GuideDistrict>();
    public ICollection<GuideCertificate> Certificates { get; set; } = new List<GuideCertificate>();
    public ICollection<GuidePhoto> Photos { get; set; } = new List<GuidePhoto>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<GuideAvailability> Availabilities { get; set; } = new List<GuideAvailability>();
    public ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
