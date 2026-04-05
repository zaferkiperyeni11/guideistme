using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetGuideBySlug;

public record GetGuideBySlugQuery(string Slug) : IRequest<GuideDetailDto>;
