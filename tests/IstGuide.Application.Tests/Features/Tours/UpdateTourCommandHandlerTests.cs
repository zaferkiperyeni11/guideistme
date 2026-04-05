using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Features.Tours.Commands.UpdateTour;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using NSubstitute;

namespace IstGuide.Application.Tests.Features.Tours;

public class UpdateTourCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateTour()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tour = new Tour
        {
            Id = tourId,
            GuideId = Guid.NewGuid(),
            Title = "Old Title",
            Description = "Old Description",
            Price = 50.00m,
            Duration = "2 Hours",
            Slug = "old-title"
        };

        var tourRepository = Substitute.For<ITourRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new UpdateTourCommand
        {
            TourId = tourId,
            Title = "New Title",
            Description = "New Description",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        tourRepository.GetByIdAsync(tourId, Arg.Any<CancellationToken>()).Returns(tour);

        var handler = new UpdateTourCommandHandler(tourRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal("New Title", tour.Title);
        Assert.Equal("New Description", tour.Description);
        Assert.Equal(99.99m, tour.Price);
        Assert.Equal("4 Hours", tour.Duration);
        await tourRepository.Received(1).UpdateAsync(tour, Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentTour_ShouldThrowNotFoundException()
    {
        // Arrange
        var tourId = Guid.NewGuid();

        var tourRepository = Substitute.For<ITourRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new UpdateTourCommand
        {
            TourId = tourId,
            Title = "New Title",
            Description = "New Description",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        tourRepository.GetByIdAsync(tourId, Arg.Any<CancellationToken>()).Returns((Tour?)null);

        var handler = new UpdateTourCommandHandler(tourRepository, unitOfWork);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        await tourRepository.DidNotReceive().UpdateAsync(Arg.Any<Tour>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CanToggleIsActive()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tour = new Tour
        {
            Id = tourId,
            GuideId = Guid.NewGuid(),
            Title = "Test Tour",
            Description = "Test Description",
            Price = 50.00m,
            Duration = "2 Hours",
            Slug = "test-tour",
            IsActive = true
        };

        var tourRepository = Substitute.For<ITourRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new UpdateTourCommand
        {
            TourId = tourId,
            Title = "Test Tour",
            Description = "Test Description",
            Price = 50.00m,
            Duration = "2 Hours",
            IsActive = false
        };

        tourRepository.GetByIdAsync(tourId, Arg.Any<CancellationToken>()).Returns(tour);

        var handler = new UpdateTourCommandHandler(tourRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.False(tour.IsActive);
    }
}
