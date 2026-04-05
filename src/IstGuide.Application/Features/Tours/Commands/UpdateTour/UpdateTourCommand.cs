using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Tours.Commands.UpdateTour;

public record UpdateTourCommand : IRequest<Result>
{
    public Guid TourId { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; }
    public string Duration { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public Guid? DistrictId { get; init; }
    public bool IsActive { get; init; } = true;
}
