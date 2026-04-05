using IstGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IstGuide.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(r => r.ReviewerName).HasMaxLength(200).IsRequired();
        builder.Property(r => r.ReviewerEmail).HasMaxLength(256);
        builder.Property(r => r.Comment).HasMaxLength(1000);
        builder.Property(r => r.AdminReply).HasMaxLength(1000);
        builder.Property(r => r.ReviewerCountry).HasMaxLength(100);
        builder.Property(r => r.ReviewerLanguage).HasMaxLength(50);
        builder.Property(r => r.CreatedBy).HasMaxLength(256);
        builder.Property(r => r.UpdatedBy).HasMaxLength(256);
        builder.ToTable(t => t.HasCheckConstraint("CK_Reviews_Rating", "Rating BETWEEN 1 AND 5"));
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(c => c.VisitorName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.VisitorEmail).HasMaxLength(256).IsRequired();
        builder.Property(c => c.VisitorPhone).HasMaxLength(20);
        builder.Property(c => c.Message).HasMaxLength(2000).IsRequired();
        builder.Property(c => c.AdminNotes).HasMaxLength(1000);
        builder.Property(c => c.CreatedBy).HasMaxLength(256);
        builder.Property(c => c.UpdatedBy).HasMaxLength(256);
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.Guide)
            .WithMany()
            .HasForeignKey(c => c.GuideId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class GuideCertificateConfiguration : IEntityTypeConfiguration<GuideCertificate>
{
    public void Configure(EntityTypeBuilder<GuideCertificate> builder)
    {
        builder.HasKey(gc => gc.Id);
        builder.Property(gc => gc.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(gc => gc.CertificateName).HasMaxLength(200).IsRequired();
        builder.Property(gc => gc.IssuingAuthority).HasMaxLength(200);
        builder.Property(gc => gc.DocumentUrl).HasMaxLength(500);
        builder.HasQueryFilter(gc => !gc.IsDeleted);
    }
}

public class GuidePhotoConfiguration : IEntityTypeConfiguration<GuidePhoto>
{
    public void Configure(EntityTypeBuilder<GuidePhoto> builder)
    {
        builder.HasKey(gp => gp.Id);
        builder.Property(gp => gp.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(gp => gp.Url).HasMaxLength(500).IsRequired();
        builder.Property(gp => gp.ThumbnailUrl).HasMaxLength(500);
        builder.Property(gp => gp.Caption).HasMaxLength(200);
        builder.HasQueryFilter(gp => !gp.IsDeleted);
    }
}

public class PageContentConfiguration : IEntityTypeConfiguration<PageContent>
{
    public void Configure(EntityTypeBuilder<PageContent> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(p => p.Key).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.MetaTitle).HasMaxLength(200);
        builder.Property(p => p.MetaDescription).HasMaxLength(500);
        builder.Property(p => p.CreatedBy).HasMaxLength(256);
        builder.Property(p => p.UpdatedBy).HasMaxLength(256);
        builder.HasIndex(p => p.Key).IsUnique();
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

public class SiteSettingsConfiguration : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(s => s.Key).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.GroupName).HasMaxLength(100).IsRequired();
        builder.HasIndex(s => s.Key).IsUnique();
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}

public class GuideAvailabilityConfiguration : IEntityTypeConfiguration<GuideAvailability>
{
    public void Configure(EntityTypeBuilder<GuideAvailability> builder)
    {
        builder.HasKey(ga => ga.Id);
        builder.Property(ga => ga.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(ga => ga.StartTime).IsRequired();
        builder.Property(ga => ga.EndTime).IsRequired();
        builder.HasQueryFilter(ga => !ga.IsDeleted);

        builder.HasOne(ga => ga.Guide)
            .WithMany(g => g.Availabilities)
            .HasForeignKey(ga => ga.GuideId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(t => t.Title).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Slug).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(2000).IsRequired();
        builder.Property(t => t.Price).HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(t => t.Duration).HasMaxLength(100).IsRequired();
        builder.Property(t => t.ImageUrl).HasMaxLength(500);

        builder.HasIndex(t => t.Slug).IsUnique();
        builder.HasQueryFilter(t => !t.IsDeleted);

        // Tour bir Guide'a ait olmalı (cascade delete)
        builder.HasOne(t => t.Guide)
            .WithMany(g => g.Tours)
            .HasForeignKey(t => t.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        // Tour bir District'e bağlı olabilir (nullable)
        builder.HasOne(t => t.District)
            .WithMany(d => d.Tours)
            .HasForeignKey(t => t.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
