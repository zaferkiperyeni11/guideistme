using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.ContactRequests.Queries.GetContactRequestStats;

public record GetContactRequestStatsQuery : IRequest<ContactRequestStatsDto>;

public record ContactRequestStatsDto
{
    public int TotalRequests { get; init; }
    public int NewRequests { get; init; }
    public int ViewedRequests { get; init; }
    public int RepliedRequests { get; init; }
    public int ConvertedRequests { get; init; }
    public int ClosedRequests { get; init; }
}
