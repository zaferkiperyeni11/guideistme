using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Tests.Entities;

public class TourTests
{
    [Fact]
    public void Should_Create_Tour_With_Valid_Data()
    {
        var tour = new Tour
        {
            Title = "Bosphorus Tour",
            Slug = "bosphorus-tour",
            Description = "Amazing Bosphorus experience",
            Price = 250m,
            Duration = "3 Hours",
            IsActive = true,
            IsFeatured = true
        };

        Assert.Equal("Bosphorus Tour", tour.Title);
        Assert.Equal(250m, tour.Price);
        Assert.True(tour.IsActive);
        Assert.True(tour.IsFeatured);
    }

    [Fact]
    public void Should_Have_Default_Values()
    {
        var tour = new Tour();

        Assert.True(tour.IsActive);
        Assert.False(tour.IsFeatured);
        Assert.Equal(string.Empty, tour.Title);
        Assert.Equal(string.Empty, tour.Slug);
        Assert.Equal(string.Empty, tour.Description);
        Assert.Equal(0m, tour.Price);
        Assert.Equal(string.Empty, tour.Duration);
    }

    [Fact]
    public void Should_Link_To_Guide_And_District()
    {
        var guide = new Guide
        {
            FirstName = "Test",
            LastName = "Guide",
            Email = "test@test.com",
            PhoneNumber = "+905000000000",
            Slug = "test-guide",
            Title = "Guide",
            Bio = "Bio",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        var district = new District
        {
            Name = "Sultanahmet",
            Slug = "sultanahmet"
        };

        var tour = new Tour
        {
            Title = "Test Tour",
            Slug = "test-tour",
            Description = "Test",
            Price = 100m,
            Duration = "2 Hours",
            Guide = guide,
            GuideId = guide.Id,
            District = district,
            DistrictId = district.Id
        };

        Assert.Equal(guide.Id, tour.GuideId);
        Assert.Equal(district.Id, tour.DistrictId);
        Assert.Same(guide, tour.Guide);
        Assert.Same(district, tour.District);
    }
}
