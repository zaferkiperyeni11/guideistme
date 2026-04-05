using IstGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IstGuide.Persistence.Configurations;

public class GuideConfiguration : IEntityTypeConfiguration<Guide>
{
    public void Configure(EntityTypeBuilder<Guide> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(g => g.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(g => g.LastName).HasMaxLength(100).IsRequired();
        builder.Property(g => g.Email).HasMaxLength(256).IsRequired();
        builder.Property(g => g.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(g => g.WhatsAppUrl).HasMaxLength(200);
        builder.Property(g => g.ProfilePhotoUrl).HasMaxLength(500);
        builder.Property(g => g.CoverPhotoUrl).HasMaxLength(500);
        builder.Property(g => g.Slug).HasMaxLength(200).IsRequired();
        builder.Property(g => g.Title).HasMaxLength(200).IsRequired();
        builder.Property(g => g.Bio).HasMaxLength(500).IsRequired();
        builder.Property(g => g.LicenseNumber).HasMaxLength(50);
        builder.Property(g => g.HourlyRate).HasColumnType("decimal(10,2)");
        builder.Property(g => g.DailyRate).HasColumnType("decimal(10,2)");
        builder.Property(g => g.UserId).HasMaxLength(450);
        builder.Property(g => g.CreatedBy).HasMaxLength(256);
        builder.Property(g => g.UpdatedBy).HasMaxLength(256);

        builder.HasIndex(g => g.Email).IsUnique();
        builder.HasIndex(g => g.Slug).IsUnique();

        builder.HasQueryFilter(g => !g.IsDeleted);

        builder.HasMany(g => g.Languages)
            .WithOne(gl => gl.Guide)
            .HasForeignKey(gl => gl.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Specialties)
            .WithOne(gs => gs.Guide)
            .HasForeignKey(gs => gs.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.ServiceDistricts)
            .WithOne(gd => gd.Guide)
            .HasForeignKey(gd => gd.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Certificates)
            .WithOne(gc => gc.Guide)
            .HasForeignKey(gc => gc.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Photos)
            .WithOne(gp => gp.Guide)
            .HasForeignKey(gp => gp.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Reviews)
            .WithOne(r => r.Guide)
            .HasForeignKey(r => r.GuideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Availabilities)
            .WithOne(ga => ga.Guide)
            .HasForeignKey(ga => ga.GuideId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
