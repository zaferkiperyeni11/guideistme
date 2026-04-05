using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Features.Tours.Commands.DeleteTour;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using NSubstitute;

namespace IstGuide.Application.Tests.Features.Tours;

public class DeleteTourCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidTourId_ShouldSoftDeleteTour()
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
            IsDeleted = false
        };

        var tourRepository = Substitute.For<ITourRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new DeleteTourCommand(tourId);

        tourRepository.GetByIdAsync(tourId, Arg.Any<CancellationToken>()).Returns(tour);

        var handler = new DeleteTourCommandHandler(tourRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.True(tour.IsDeleted);
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

        var command = new DeleteTourCommand(tourId);

        tourRepository.GetByIdAsync(tourId, Arg.Any<CancellationToken>()).Returns((Tour?)null);

        var handler = new DeleteTourCommandHandler(tourRepository, unitOfWork);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        await tourRepository.DidNotReceive().UpdateAsync(Arg.Any<Tour>(), Arg.Any<CancellationToken>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithEmptyTourId_ShouldThrowException()
    {
        // Arrange
        var tourRepository = Substitute.For<ITourRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new DeleteTourCommand(Guid.Empty);

        tourRepository.GetByIdAsync(Guid.Empty, Arg.Any<CancellationToken>()).Returns((Tour?)null);

        var handler = new DeleteTourCommandHandler(tourRepository, unitOfWork);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
