using MediatR;

namespace IstGuide.Application.Features.Districts.Queries.GetAllDistricts;

public record GetAllDistrictsQuery(bool? PopularOnly = null) : IRequest<IReadOnlyList<DistrictDto>>;

public class DistrictDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPopular { get; set; }
    public int SortOrder { get; set; }
}
