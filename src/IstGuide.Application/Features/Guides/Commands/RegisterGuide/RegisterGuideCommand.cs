using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.RegisterGuide;

public record RegisterGuideCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Bio { get; init; } = default!;
    public string? DetailedDescription { get; init; }
    public int YearsOfExperience { get; init; }
    public Gender Gender { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string? LicenseNumber { get; init; }
    public List<Guid> LanguageIds { get; init; } = new();
    public List<Guid> SpecialtyIds { get; init; } = new();
    public List<Guid> DistrictIds { get; init; } = new();
}
