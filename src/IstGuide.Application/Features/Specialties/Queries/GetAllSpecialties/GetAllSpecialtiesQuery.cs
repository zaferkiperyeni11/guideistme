using MediatR;

namespace IstGuide.Application.Features.Specialties.Queries.GetAllSpecialties;

public record GetAllSpecialtiesQuery : IRequest<IReadOnlyList<SpecialtyDto>>;

public class SpecialtyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; }
}
