using FluentValidation.TestHelper;
using IstGuide.Application.Features.Guides.Commands.DeleteGuide;

namespace IstGuide.Application.Tests.Features.Guides;

public class DeleteGuideCommandValidatorTests
{
    private readonly DeleteGuideCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidGuideId_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new DeleteGuideCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyGuideId_ShouldHaveError()
    {
        // Arrange
        var command = new DeleteGuideCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuideId);
    }
}
