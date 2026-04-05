using MediatR;

namespace IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;

public record GetGuideReviewsQuery(Guid GuideId) : IRequest<IReadOnlyList<ReviewDto>>;
