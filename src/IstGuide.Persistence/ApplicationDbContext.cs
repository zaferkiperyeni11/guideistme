using IstGuide.Application.Common.Interfaces;
using IstGuide.Domain.Entities;
using IstGuide.Infrastructure.Identity;
using IstGuide.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly AuditableEntityInterceptor _auditInterceptor;
    private readonly SoftDeleteInterceptor _softDeleteInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntityInterceptor auditInterceptor,
        SoftDeleteInterceptor softDeleteInterceptor) : base(options)
    {
        _auditInterceptor = auditInterceptor;
        _softDeleteInterceptor = softDeleteInterceptor;
    }

    public DbSet<Guide> Guides => Set<Guide>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<GuideLanguage> GuideLanguages => Set<GuideLanguage>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<GuideSpecialty> GuideSpecialties => Set<GuideSpecialty>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<GuideDistrict> GuideDistricts => Set<GuideDistrict>();
    public DbSet<GuideCertificate> GuideCertificates => Set<GuideCertificate>();
    public DbSet<GuidePhoto> GuidePhotos => Set<GuidePhoto>();
    public DbSet<GuideAvailability> GuideAvailabilities => Set<GuideAvailability>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
    public DbSet<PageContent> PageContents => Set<PageContent>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<Tour> Tours => Set<Tour>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor, _softDeleteInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
