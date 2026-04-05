using FluentValidation.TestHelper;
using IstGuide.Application.Features.Tours.Commands.CreateTour;
using IstGuide.Application.Features.Tours.Commands.DeleteTour;
using IstGuide.Application.Features.Tours.Commands.UpdateTour;

namespace IstGuide.Application.Tests.Features.Tours;

public class TourCommandValidatorTests
{
    [Fact]
    public void CreateTourValidator_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var validator = new CreateTourCommandValidator();
        var command = new CreateTourCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Istanbul Tour",
            Description = "Explore Istanbul",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateTourValidator_WithEmptyTitle_ShouldHaveError()
    {
        // Arrange
        var validator = new CreateTourCommandValidator();
        var command = new CreateTourCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "",
            Description = "Explore Istanbul",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void CreateTourValidator_WithZeroPrice_ShouldHaveError()
    {
        // Arrange
        var validator = new CreateTourCommandValidator();
        var command = new CreateTourCommand
        {
            GuideId = Guid.NewGuid(),
            Title = "Istanbul Tour",
            Description = "Explore Istanbul",
            Price = 0m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void CreateTourValidator_WithLongTitle_ShouldHaveError()
    {
        // Arrange
        var validator = new CreateTourCommandValidator();
        var command = new CreateTourCommand
        {
            GuideId = Guid.NewGuid(),
            Title = new string('a', 201),
            Description = "Explore Istanbul",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void UpdateTourValidator_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var validator = new UpdateTourCommandValidator();
        var command = new UpdateTourCommand
        {
            TourId = Guid.NewGuid(),
            Title = "Istanbul Tour",
            Description = "Explore Istanbul",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateTourValidator_WithNegativePrice_ShouldHaveError()
    {
        // Arrange
        var validator = new UpdateTourCommandValidator();
        var command = new UpdateTourCommand
        {
            TourId = Guid.NewGuid(),
            Title = "Istanbul Tour",
            Description = "Explore Istanbul",
            Price = -10m,
            Duration = "4 Hours"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void DeleteTourValidator_WithValidId_ShouldNotHaveErrors()
    {
        // Arrange
        var validator = new DeleteTourCommandValidator();
        var command = new DeleteTourCommand(Guid.NewGuid());

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeleteTourValidator_WithEmptyId_ShouldHaveError()
    {
        // Arrange
        var validator = new DeleteTourCommandValidator();
        var command = new DeleteTourCommand(Guid.Empty);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TourId);
    }
}
