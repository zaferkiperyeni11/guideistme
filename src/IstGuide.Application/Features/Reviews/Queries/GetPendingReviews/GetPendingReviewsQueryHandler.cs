using AutoMapper;
using IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Queries.GetPendingReviews;

public class GetPendingReviewsQueryHandler : IRequestHandler<GetPendingReviewsQuery, IReadOnlyList<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetPendingReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ReviewDto>> Handle(GetPendingReviewsQuery request, CancellationToken ct)
    {
        var reviews = await _reviewRepository.GetPendingReviewsAsync(ct);
        return _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
    }
}
