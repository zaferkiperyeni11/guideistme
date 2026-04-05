using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Persistence.Seeds;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedLanguagesAsync(context);
        await SeedSpecialtiesAsync(context);
        await SeedDistrictsAsync(context);
        await SeedSiteSettingsAsync(context);
        await SeedGuidesAsync(context);
        await SeedToursAsync(context);
        await SeedAdminUserAsync(userManager, roleManager);
    }

    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        const string adminEmail = "admin@istguide.me";
        const string adminPassword = "Admin123!";
        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
            await roleManager.CreateAsync(new IdentityRole(adminRole));

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, adminRole);
        }
        else if (!await userManager.IsInRoleAsync(existingAdmin, adminRole))
        {
            await userManager.AddToRoleAsync(existingAdmin, adminRole);
        }
    }

    private static async Task SeedLanguagesAsync(ApplicationDbContext context)
    {
        if (await context.Languages.AnyAsync()) return;

        var languages = new List<Language>
        {
            new() { Id = Guid.NewGuid(), Name = "İngilizce",  Code = "en", NativeName = "English",    CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Almanca",    Code = "de", NativeName = "Deutsch",    CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Fransızca",  Code = "fr", NativeName = "Français",   CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "İspanyolca", Code = "es", NativeName = "Español",    CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Arapça",     Code = "ar", NativeName = "العربية",    CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Rusça",      Code = "ru", NativeName = "Русский",    CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Japonca",    Code = "ja", NativeName = "日本語",       CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Korece",     Code = "ko", NativeName = "한국어",       CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Çince",      Code = "zh", NativeName = "中文",         CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Portekizce", Code = "pt", NativeName = "Português",  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "İtalyanca",  Code = "it", NativeName = "Italiano",   CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Türkçe",     Code = "tr", NativeName = "Türkçe",     CreatedAt = DateTime.UtcNow },
        };

        await context.Languages.AddRangeAsync(languages);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSpecialtiesAsync(ApplicationDbContext context)
    {
        if (await context.Specialties.AnyAsync()) return;

        var specialties = new List<Specialty>
        {
            new() { Id = Guid.NewGuid(), Name = "Tarihi Turlar",     Slug = "tarihi-turlar",      SortOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Gastronomi Turları", Slug = "gastronomi-turlari", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Boğaz Turları",      Slug = "bogaz-turlari",      SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Fotoğraf Turları",   Slug = "fotograf-turlari",   SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Müze & Sanat",       Slug = "muze-sanat",         SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Mimari Turlar",      Slug = "mimari-turlar",      SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Alışveriş Turları",  Slug = "alisveris-turlari",  SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Gece Turları",       Slug = "gece-turlari",       SortOrder = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Doğa & Trekking",    Slug = "doga-trekking",      SortOrder = 9, IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Özel VIP Turlar",    Slug = "ozel-vip-turlar",    SortOrder = 10, IsActive = true, CreatedAt = DateTime.UtcNow },
        };

        await context.Specialties.AddRangeAsync(specialties);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDistrictsAsync(ApplicationDbContext context)
    {
        if (await context.Districts.AnyAsync()) return;

        var districts = new List<District>
        {
            new() { Id = Guid.NewGuid(), Name = "Sultanahmet",      Slug = "sultanahmet",      IsPopular = true,  SortOrder = 1,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Eminönü",          Slug = "eminonu",           IsPopular = true,  SortOrder = 2,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Beyoğlu / Taksim", Slug = "beyoglu-taksim",   IsPopular = true,  SortOrder = 3,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Karaköy / Galata", Slug = "karakoy-galata",   IsPopular = true,  SortOrder = 4,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Beşiktaş",         Slug = "besiktas",         IsPopular = true,  SortOrder = 5,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Kadıköy",          Slug = "kadikoy",          IsPopular = true,  SortOrder = 6,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Üsküdar",          Slug = "uskudar",          IsPopular = true,  SortOrder = 7,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Balat / Fener",    Slug = "balat-fener",      IsPopular = true,  SortOrder = 8,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Ortaköy",          Slug = "ortakoy",          IsPopular = false, SortOrder = 9,  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Sarıyer",          Slug = "sariyer",          IsPopular = false, SortOrder = 10, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Adalar",           Slug = "adalar",           IsPopular = true,  SortOrder = 11, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Eyüpsultan",       Slug = "eyupsultan",       IsPopular = false, SortOrder = 12, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Fatih",            Slug = "fatih",            IsPopular = false, SortOrder = 13, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Bakırköy",         Slug = "bakirkoy",         IsPopular = false, SortOrder = 14, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Bebek",            Slug = "bebek",            IsPopular = false, SortOrder = 15, CreatedAt = DateTime.UtcNow },
        };

        await context.Districts.AddRangeAsync(districts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSiteSettingsAsync(ApplicationDbContext context)
    {
        if (await context.SiteSettings.AnyAsync()) return;

        var settings = new List<SiteSettings>
        {
            new() { Id = Guid.NewGuid(), Key = "site_name",       Value = "IstGuide",                       GroupName = "General", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "site_tagline",    Value = "İstanbul Rehber Platformu",       GroupName = "General", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "contact_email",   Value = "info@istguide.me",               GroupName = "Contact", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "contact_phone",   Value = "+902121234567",                  GroupName = "Contact", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "whatsapp_number", Value = "+905321234567",                  GroupName = "Contact", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "instagram_url",   Value = "https://instagram.com/istguide", GroupName = "Social",  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "facebook_url",    Value = "",                               GroupName = "Social",  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Key = "twitter_url",     Value = "",                               GroupName = "Social",  CreatedAt = DateTime.UtcNow },
        };

        await context.SiteSettings.AddRangeAsync(settings);
        await context.SaveChangesAsync();
    }

    private static async Task SeedToursAsync(ApplicationDbContext context)
    {
        if (await context.Tours.AnyAsync()) return;

        var guide = await context.Guides.FirstOrDefaultAsync();
        var sultanahmet = await context.Districts.FirstOrDefaultAsync(d => d.Slug == "sultanahmet");

        var tours = new List<Tour> {
            new() { 
                Id = Guid.NewGuid(), 
                Title = "Old City & Hagia Sophia", 
                Slug = "old-city-hagia-sophia",
                Description = "Discover the legendary treasures of Istanbul", 
                Price = 150, 
                Duration = "4 Hours",
                IsActive = true,
                IsFeatured = true,
                GuideId = guide?.Id ?? Guid.NewGuid(),
                DistrictId = sultanahmet?.Id,
                CreatedAt = DateTime.UtcNow 
            }
        };
        await context.Tours.AddRangeAsync(tours);
        await context.SaveChangesAsync();
    }

    private static async Task SeedGuidesAsync(ApplicationDbContext context)
    {
        if (await context.Guides.AnyAsync()) return;
        var guide = new Guide {
            Id = Guid.NewGuid(),
            FirstName = "Zafer",
            LastName = "Admin",
            Email = "admin@istguide.me",
            PhoneNumber = "+905000000000",
            Slug = "zafer-admin-istanbul",
            Title = "Elite Istanbul Historian",
            Bio = "Professional guide with 10 years experience",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            YearsOfExperience = 10,
            Status = GuideStatus.Approved,
            CreatedAt = DateTime.UtcNow
        };
        await context.Guides.AddAsync(guide);
        await context.SaveChangesAsync();
    }
}
