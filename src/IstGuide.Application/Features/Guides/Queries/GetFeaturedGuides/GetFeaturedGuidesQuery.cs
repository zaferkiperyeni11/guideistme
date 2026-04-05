using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetFeaturedGuides;

public record GetFeaturedGuidesQuery : IRequest<IReadOnlyList<GuideListDto>>;
