using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;

namespace IstGuide.Domain.Tests.Entities;

public class GuideTests
{
    [Fact]
    public void Should_Create_Guide_With_Valid_Data()
    {
        var guide = new Guide
        {
            FirstName = "Ahmet",
            LastName = "Yilmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+905321234567",
            Slug = "ahmet-yilmaz-istanbul",
            Title = "Lisansli Turist Rehberi",
            Bio = "10 yillik deneyim",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            YearsOfExperience = 10,
            Status = GuideStatus.Pending
        };

        Assert.Equal("Ahmet", guide.FirstName);
        Assert.Equal("ahmet@example.com", guide.Email);
        Assert.Equal(GuideStatus.Pending, guide.Status);
        Assert.False(guide.IsFeatured);
        Assert.Equal(0, guide.ReviewCount);
    }

    [Fact]
    public void Should_Have_Empty_Collections_On_Init()
    {
        var guide = new Guide
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            PhoneNumber = "+905000000000",
            Slug = "test-user",
            Title = "Rehber",
            Bio = "Test bio",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        Assert.NotNull(guide.Languages);
        Assert.NotNull(guide.Specialties);
        Assert.NotNull(guide.ServiceDistricts);
        Assert.NotNull(guide.Certificates);
        Assert.NotNull(guide.Photos);
        Assert.NotNull(guide.Reviews);
        Assert.NotNull(guide.Availabilities);
        Assert.NotNull(guide.Tours);

        Assert.Empty(guide.Languages);
        Assert.Empty(guide.Specialties);
        Assert.Empty(guide.Tours);
    }

    [Fact]
    public void Should_Generate_WhatsAppUrl_From_PhoneNumber()
    {
        var phoneNumber = new IstGuide.Domain.ValueObjects.PhoneNumber("+90", "5321234567");

        Assert.Equal("https://wa.me/905321234567", phoneNumber.ToWhatsAppUrl());
        Assert.Equal("+90 5321234567", phoneNumber.ToFormatted());
    }
}
