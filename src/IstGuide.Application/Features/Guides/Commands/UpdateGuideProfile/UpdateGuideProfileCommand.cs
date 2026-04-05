using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.UpdateGuideProfile;

public record UpdateGuideProfileCommand : IRequest<Result>
{
    public Guid GuideId { get; init; }
    public string Title { get; init; } = default!;
    public string Bio { get; init; } = default!;
    public string? DetailedDescription { get; init; }
    public int YearsOfExperience { get; init; }
    public string? LicenseNumber { get; init; }
    public List<Guid> LanguageIds { get; init; } = new();
    public List<Guid> SpecialtyIds { get; init; } = new();
    public List<Guid> DistrictIds { get; init; } = new();
}
