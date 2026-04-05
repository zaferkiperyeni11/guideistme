using IstGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Guide> Guides { get; }
    DbSet<Language> Languages { get; }
    DbSet<GuideLanguage> GuideLanguages { get; }
    DbSet<Specialty> Specialties { get; }
    DbSet<GuideSpecialty> GuideSpecialties { get; }
    DbSet<District> Districts { get; }
    DbSet<GuideDistrict> GuideDistricts { get; }
    DbSet<GuideCertificate> GuideCertificates { get; }
    DbSet<GuidePhoto> GuidePhotos { get; }
    DbSet<GuideAvailability> GuideAvailabilities { get; }
    DbSet<Review> Reviews { get; }
    DbSet<ContactRequest> ContactRequests { get; }
    DbSet<PageContent> PageContents { get; }
    DbSet<SiteSettings> SiteSettings { get; }
    DbSet<Tour> Tours { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
