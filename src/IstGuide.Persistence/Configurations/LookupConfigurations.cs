using IstGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IstGuide.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Code).HasMaxLength(10).IsRequired();
        builder.Property(l => l.NativeName).HasMaxLength(100).IsRequired();
        builder.Property(l => l.FlagIconUrl).HasMaxLength(500);
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}

public class GuideLanguageConfiguration : IEntityTypeConfiguration<GuideLanguage>
{
    public void Configure(EntityTypeBuilder<GuideLanguage> builder)
    {
        builder.HasKey(gl => gl.Id);
        builder.Property(gl => gl.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.HasIndex(gl => new { gl.GuideId, gl.LanguageId }).IsUnique();
        builder.HasQueryFilter(gl => !gl.IsDeleted);

        builder.HasOne(gl => gl.Language)
            .WithMany(l => l.GuideLanguages)
            .HasForeignKey(gl => gl.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Slug).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.IconUrl).HasMaxLength(500);
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}

public class GuideSpecialtyConfiguration : IEntityTypeConfiguration<GuideSpecialty>
{
    public void Configure(EntityTypeBuilder<GuideSpecialty> builder)
    {
        builder.HasKey(gs => gs.Id);
        builder.Property(gs => gs.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.HasIndex(gs => new { gs.GuideId, gs.SpecialtyId }).IsUnique();
        builder.HasQueryFilter(gs => !gs.IsDeleted);

        builder.HasOne(gs => gs.Specialty)
            .WithMany(s => s.GuideSpecialties)
            .HasForeignKey(gs => gs.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Slug).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.ImageUrl).HasMaxLength(500);
        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}

public class GuideDistrictConfiguration : IEntityTypeConfiguration<GuideDistrict>
{
    public void Configure(EntityTypeBuilder<GuideDistrict> builder)
    {
        builder.HasKey(gd => gd.Id);
        builder.Property(gd => gd.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.HasIndex(gd => new { gd.GuideId, gd.DistrictId }).IsUnique();
        builder.HasQueryFilter(gd => !gd.IsDeleted);

        builder.HasOne(gd => gd.District)
            .WithMany(d => d.GuideDistricts)
            .HasForeignKey(gd => gd.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
