using IstGuide.Application.Common.Models;
using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.SearchGuides;

public record SearchGuidesQuery : IRequest<PaginatedList<GuideListDto>>
{
    public GuideSearchCriteria Criteria { get; init; } = new();
}
