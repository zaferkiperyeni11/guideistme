using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Features.Guides.Commands.DeleteGuide;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using NSubstitute;

namespace IstGuide.Application.Tests.Features.Guides;

public class DeleteGuideCommandHandlerTests
{
    private readonly IGuideRepository _guideRepository = Substitute.For<IGuideRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_WithValidGuideId_ShouldSoftDeleteGuide()
    {
        // Arrange
        var guideId = Guid.NewGuid();
        var guide = new Guide
        {
            Id = guideId,
            FirstName = "Ahmet",
            LastName = "Yilmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+905321234567",
            Slug = "ahmet-yilmaz",
            Title = "Guide",
            Bio = "Bio",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            YearsOfExperience = 10,
            Status = GuideStatus.Approved,
            IsDeleted = false
        };

        _guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns(guide);

        var command = new DeleteGuideCommand(guideId);
        var handler = new DeleteGuideCommandHandler(_guideRepository, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.True(guide.IsDeleted);
        await _guideRepository.Received(1).UpdateAsync(guide, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentGuideId_ShouldThrowNotFoundException()
    {
        // Arrange
        var guideId = Guid.NewGuid();
        _guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns((Guide?)null);

        var command = new DeleteGuideCommand(guideId);
        var handler = new DeleteGuideCommandHandler(_guideRepository, _unitOfWork);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        await _guideRepository.DidNotReceive().UpdateAsync(Arg.Any<Guide>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithEmptyGuideId_ShouldThrowException()
    {
        // Arrange
        var command = new DeleteGuideCommand(Guid.Empty);
        var handler = new DeleteGuideCommandHandler(_guideRepository, _unitOfWork);

        // Act & Assert
        _guideRepository.GetByIdAsync(Guid.Empty, Arg.Any<CancellationToken>()).Returns((Guide?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateAndSaveChanges()
    {
        // Arrange
        var guideId = Guid.NewGuid();
        var guide = new Guide
        {
            Id = guideId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            PhoneNumber = "+905000000000",
            Slug = "test-user",
            Title = "Test Guide",
            Bio = "Test bio",
            DateOfBirth = new DateTime(1995, 1, 1),
            Gender = Gender.Male,
            YearsOfExperience = 5,
            IsDeleted = false
        };

        _guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns(guide);

        var command = new DeleteGuideCommand(guideId);
        var handler = new DeleteGuideCommandHandler(_guideRepository, _unitOfWork);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert - Verify call order
        await _guideRepository.Received(1).GetByIdAsync(guideId, Arg.Any<CancellationToken>());
        await _guideRepository.Received(1).UpdateAsync(guide, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
