using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Specialties.Commands.UpdateSpecialty;

public record UpdateSpecialtyCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string? IconUrl { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}
