using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Features.Tours.Commands.CreateTour;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using NSubstitute;

namespace IstGuide.Application.Tests.Features.Tours;

public class CreateTourCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateTour()
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
            Gender = Domain.Enums.Gender.Male
        };

        var tourRepository = Substitute.For<ITourRepository>();
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new CreateTourCommand
        {
            GuideId = guideId,
            Title = "Byzantine Istanbul Tour",
            Description = "Explore the historic monuments of Constantinople",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns(guide);
        slugService.GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("byzantine-istanbul-tour");

        var handler = new CreateTourCommandHandler(tourRepository, guideRepository, slugService, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotEqual(Guid.Empty, result.Value);
        await tourRepository.Received(1).AddAsync(Arg.Any<Tour>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentGuide_ShouldReturnFailure()
    {
        // Arrange
        var guideId = Guid.NewGuid();

        var tourRepository = Substitute.For<ITourRepository>();
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new CreateTourCommand
        {
            GuideId = guideId,
            Title = "Byzantine Istanbul Tour",
            Description = "Explore the historic monuments of Constantinople",
            Price = 99.99m,
            Duration = "4 Hours"
        };

        guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns((Guide?)null);

        var handler = new CreateTourCommandHandler(tourRepository, guideRepository, slugService, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.NotEmpty(result.Errors);
        await tourRepository.DidNotReceive().AddAsync(Arg.Any<Tour>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldGenerateSlug()
    {
        // Arrange
        var guideId = Guid.NewGuid();
        var guide = new Guide
        {
            Id = guideId,
            FirstName = "Test",
            LastName = "Guide",
            Email = "test@example.com",
            PhoneNumber = "+905001234567",
            Slug = "test-guide",
            Title = "Guide",
            Bio = "Bio",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Domain.Enums.Gender.Male
        };

        var tourRepository = Substitute.For<ITourRepository>();
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var command = new CreateTourCommand
        {
            GuideId = guideId,
            Title = "Test Tour",
            Description = "Test Description",
            Price = 50.00m,
            Duration = "2 Hours"
        };

        guideRepository.GetByIdAsync(guideId, Arg.Any<CancellationToken>()).Returns(guide);
        slugService.GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("test-tour");

        var handler = new CreateTourCommandHandler(tourRepository, guideRepository, slugService, unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        await slugService.Received(1).GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
