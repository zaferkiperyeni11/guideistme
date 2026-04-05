using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Features.Guides.Commands.RegisterGuide;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using NSubstitute;

namespace IstGuide.Application.Tests.Features.Guides;

public class CreateGuideCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateGuide()
    {
        // Arrange
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var context = Substitute.For<IApplicationDbContext>();

        var command = new RegisterGuideCommand
        {
            FirstName = "Ahmet",
            LastName = "Yilmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+905321234567",
            Title = "Licensed Tour Guide",
            Bio = "10 years of experience",
            YearsOfExperience = 10,
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1990, 1, 1),
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        guideRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((Guide?)null);
        slugService.GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("ahmet-yilmaz-istanbul");

        var handler = new RegisterGuideCommandHandler(guideRepository, slugService, unitOfWork, context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotEqual(Guid.Empty, result.Value);
        await guideRepository.Received(1).AddAsync(Arg.Any<Guide>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var context = Substitute.For<IApplicationDbContext>();

        var existingGuide = new Guide
        {
            Id = Guid.NewGuid(),
            FirstName = "Existing",
            LastName = "User",
            Email = "ahmet@example.com",
            PhoneNumber = "+905000000000",
            Slug = "existing-user",
            Title = "Guide",
            Bio = "Bio",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        var command = new RegisterGuideCommand
        {
            FirstName = "Ahmet",
            LastName = "Yilmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+905321234567",
            Title = "Licensed Tour Guide",
            Bio = "10 years of experience",
            YearsOfExperience = 10,
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1990, 1, 1),
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        guideRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(existingGuide);

        var handler = new RegisterGuideCommandHandler(guideRepository, slugService, unitOfWork, context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.NotEmpty(result.Errors);
        await guideRepository.DidNotReceive().AddAsync(Arg.Any<Guide>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_GuideShouldHavePendingStatus()
    {
        // Arrange
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var context = Substitute.For<IApplicationDbContext>();

        var command = new RegisterGuideCommand
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            PhoneNumber = "+905000000000",
            Title = "Guide",
            Bio = "Test bio",
            YearsOfExperience = 5,
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1995, 6, 15),
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        guideRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((Guide?)null);
        slugService.GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("test-user-istanbul");

        var handler = new RegisterGuideCommandHandler(guideRepository, slugService, unitOfWork, context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        await guideRepository.Received(1).AddAsync(
            Arg.Is<Guide>(g => g.Status == GuideStatus.Pending),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_SlugShouldBeGenerated()
    {
        // Arrange
        var guideRepository = Substitute.For<IGuideRepository>();
        var slugService = Substitute.For<ISlugService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var context = Substitute.For<IApplicationDbContext>();

        var command = new RegisterGuideCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+905001234567",
            Title = "Professional Guide",
            Bio = "Professional bio",
            YearsOfExperience = 8,
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1992, 3, 20),
            LanguageIds = new List<Guid> { Guid.NewGuid() },
            SpecialtyIds = new List<Guid> { Guid.NewGuid() },
            DistrictIds = new List<Guid> { Guid.NewGuid() }
        };

        guideRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((Guide?)null);
        slugService.GenerateUniqueSlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("john-doe-istanbul");

        var handler = new RegisterGuideCommandHandler(guideRepository, slugService, unitOfWork, context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        await slugService.Received(1).GenerateUniqueSlugAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
