using FluentAssertions;
using FluentValidation.TestHelper;
using IstGuide.Application.Features.Reviews.Commands.SubmitReview;

namespace IstGuide.Application.Tests.Features.Reviews;

public class SubmitReviewCommandValidatorTests
{
    private readonly SubmitReviewCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_GuideId_Is_Empty()
    {
        var command = new SubmitReviewCommand(
            Guid.Empty,
            "Test User",
            "test@test.com",
            5,
            "Great guide!");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.GuideId);
    }

    [Fact]
    public void Should_Have_Error_When_ReviewerName_Is_Empty()
    {
        var command = new SubmitReviewCommand(
            Guid.NewGuid(),
            string.Empty,
            "test@test.com",
            5,
            "Great guide!");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ReviewerName);
    }

    [Fact]
    public void Should_Have_Error_When_Rating_Is_Invalid()
    {
        var command = new SubmitReviewCommand(
            Guid.NewGuid(),
            "Test User",
            "test@test.com",
            6,
            "Great guide!");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Rating);
    }

    [Fact]
    public void Should_Pass_When_Email_Is_Null()
    {
        var command = new SubmitReviewCommand(
            Guid.NewGuid(),
            "Test User",
            null,
            4,
            "Good guide");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.ReviewerEmail);
    }

    [Fact]
    public void Should_Pass_When_Comment_Is_Null()
    {
        var command = new SubmitReviewCommand(
            Guid.NewGuid(),
            "Test User",
            "test@test.com",
            5,
            null);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Comment);
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var command = new SubmitReviewCommand(
            Guid.NewGuid(),
            "Test User",
            "test@test.com",
            5,
            "Excellent guide!");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
