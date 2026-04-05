using IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Queries.GetPendingReviews;

public record GetPendingReviewsQuery : IRequest<IReadOnlyList<ReviewDto>>;
