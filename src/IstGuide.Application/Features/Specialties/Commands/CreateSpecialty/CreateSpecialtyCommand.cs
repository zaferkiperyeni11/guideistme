using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Specialties.Commands.CreateSpecialty;

public record CreateSpecialtyCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string? IconUrl { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; } = true;
}
