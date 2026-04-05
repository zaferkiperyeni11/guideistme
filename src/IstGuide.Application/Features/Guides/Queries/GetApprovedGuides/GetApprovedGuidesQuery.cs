using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;

public record GetApprovedGuidesQuery : IRequest<IReadOnlyList<GuideListDto>>;
