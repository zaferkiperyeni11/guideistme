using FluentValidation.TestHelper;
using IstGuide.Application.Features.Guides.Commands.UpdateGuideProfile;

namespace IstGuide.Application.Tests.Features.Guides;

public class UpdateGuideProfileCommandValidatorTests
{
    private readonly UpdateGuideProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyGuideId_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.Empty,
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuideId);
    }

    [Fact]
    public void Validate_WithEmptyTitle_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTitleExceedingMaxLength_ShouldHaveError()
    {
        // Arrange
        var longTitle = new string('a', 201); // Exceeds 200 char limit
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = longTitle,
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithEmptyBio_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Bio);
    }

    [Fact]
    public void Validate_WithBioExceedingMaxLength_ShouldHaveError()
    {
        // Arrange
        var longBio = new string('a', 501); // Exceeds 500 char limit
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = longBio,
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Bio);
    }

    [Fact]
    public void Validate_WithNegativeYearsOfExperience_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = -1,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.YearsOfExperience);
    }

    [Fact]
    public void Validate_WithExcessiveYearsOfExperience_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 61, // Exceeds 60 year limit
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.YearsOfExperience);
    }

    [Fact]
    public void Validate_WithEmptyLanguageIds_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid>(), // Empty
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LanguageIds);
    }

    [Fact]
    public void Validate_WithEmptySpecialtyIds_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid>(), // Empty
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SpecialtyIds);
    }

    [Fact]
    public void Validate_WithEmptyDistrictIds_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateGuideProfileCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Professional Guide",
            Bio = "Experienced tour guide",
            YearsOfExperience = 10,
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid>() // Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DistrictIds);
    }
}
