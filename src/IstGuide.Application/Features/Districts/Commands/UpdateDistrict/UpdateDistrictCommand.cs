using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Districts.Commands.UpdateDistrict;

public record UpdateDistrictCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public int SortOrder { get; init; }
    public bool IsPopular { get; init; }
}
