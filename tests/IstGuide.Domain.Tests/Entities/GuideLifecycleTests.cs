using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Tests.Entities;

public class GuideLifecycleTests
{
    private Guide CreateTestGuide()
    {
        return new Guide
        {
            Id = Guid.NewGuid(),
            FirstName = "Ahmet",
            LastName = "Yilmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+905321234567",
            Slug = "ahmet-yilmaz-istanbul",
            Title = "Licensed Tour Guide",
            Bio = "10 years of experience",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            YearsOfExperience = 10,
            Status = GuideStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public void Guide_Created_ShouldHavePendingStatus()
    {
        // Arrange & Act
        var guide = CreateTestGuide();

        // Assert
        Assert.Equal(GuideStatus.Pending, guide.Status);
        Assert.False(guide.IsDeleted);
        Assert.False(guide.IsFeatured);
        Assert.Equal(0, guide.ReviewCount);
        Assert.Equal(0, guide.AverageRating);
        Assert.Equal(0, guide.ProfileViewCount);
    }

    [Fact]
    public void Guide_CanBeApproved()
    {
        // Arrange
        var guide = CreateTestGuide();

        // Act
        guide.Status = GuideStatus.Approved;

        // Assert
        Assert.Equal(GuideStatus.Approved, guide.Status);
        Assert.False(guide.IsDeleted);
    }

    [Fact]
    public void Guide_CanBeRejected()
    {
        // Arrange
        var guide = CreateTestGuide();

        // Act
        guide.Status = GuideStatus.Rejected;

        // Assert
        Assert.Equal(GuideStatus.Rejected, guide.Status);
    }

    [Fact]
    public void Guide_CanBeDeleted_SoftDelete()
    {
        // Arrange
        var guide = CreateTestGuide();
        guide.Status = GuideStatus.Approved;

        // Act
        guide.IsDeleted = true;

        // Assert
        Assert.True(guide.IsDeleted);
        Assert.Equal(GuideStatus.Approved, guide.Status); // Status doesn't change, just soft delete flag
    }

    [Fact]
    public void Guide_CanBeFeatured()
    {
        // Arrange
        var guide = CreateTestGuide();
        guide.Status = GuideStatus.Approved;

        // Act
        guide.IsFeatured = true;

        // Assert
        Assert.True(guide.IsFeatured);
    }

    [Fact]
    public void Guide_ProfileViewCountCanIncrement()
    {
        // Arrange
        var guide = CreateTestGuide();

        // Act
        guide.ProfileViewCount += 5;

        // Assert
        Assert.Equal(5, guide.ProfileViewCount);
    }

    [Fact]
    public void Guide_CanAddLanguages()
    {
        // Arrange
        var guide = CreateTestGuide();
        var languageId = Guid.NewGuid();

        // Act
        guide.Languages.Add(new GuideLanguage { GuideId = guide.Id, LanguageId = languageId });

        // Assert
        Assert.Single(guide.Languages);
        Assert.Equal(languageId, guide.Languages.First().LanguageId);
    }

    [Fact]
    public void Guide_CanAddMultipleSpecialties()
    {
        // Arrange
        var guide = CreateTestGuide();
        var spec1 = Guid.NewGuid();
        var spec2 = Guid.NewGuid();

        // Act
        guide.Specialties.Add(new GuideSpecialty { GuideId = guide.Id, SpecialtyId = spec1 });
        guide.Specialties.Add(new GuideSpecialty { GuideId = guide.Id, SpecialtyId = spec2 });

        // Assert
        Assert.Equal(2, guide.Specialties.Count);
    }

    [Fact]
    public void Guide_CanAddServiceDistricts()
    {
        // Arrange
        var guide = CreateTestGuide();
        var district1 = Guid.NewGuid();
        var district2 = Guid.NewGuid();

        // Act
        guide.ServiceDistricts.Add(new GuideDistrict { GuideId = guide.Id, DistrictId = district1 });
        guide.ServiceDistricts.Add(new GuideDistrict { GuideId = guide.Id, DistrictId = district2 });

        // Assert
        Assert.Equal(2, guide.ServiceDistricts.Count);
    }

    [Fact]
    public void Guide_CanReceiveReviews()
    {
        // Arrange
        var guide = CreateTestGuide();

        // Act
        guide.ReviewCount = 5;
        guide.AverageRating = 4.5;

        // Assert
        Assert.Equal(5, guide.ReviewCount);
        Assert.Equal(4.5, guide.AverageRating);
    }

    [Fact]
    public void Guide_CanHaveUpdates()
    {
        // Arrange
        var guide = CreateTestGuide();
        var originalTitle = guide.Title;

        // Act
        guide.Title = "Updated Title";
        guide.Bio = "Updated bio with more experience";
        guide.UpdatedAt = DateTime.UtcNow;

        // Assert
        Assert.NotEqual(originalTitle, guide.Title);
        Assert.NotNull(guide.UpdatedAt);
    }

    [Fact]
    public void DeletedGuide_ShouldPreserveAllData()
    {
        // Arrange
        var guide = CreateTestGuide();
        guide.Status = GuideStatus.Approved;
        guide.IsFeatured = true;
        guide.ReviewCount = 10;
        guide.AverageRating = 4.8;

        // Act
        guide.IsDeleted = true;

        // Assert - All data preserved
        Assert.True(guide.IsDeleted);
        Assert.Equal("Ahmet", guide.FirstName);
        Assert.Equal(GuideStatus.Approved, guide.Status);
        Assert.True(guide.IsFeatured);
        Assert.Equal(10, guide.ReviewCount);
        Assert.Equal(4.8, guide.AverageRating);
    }
}
