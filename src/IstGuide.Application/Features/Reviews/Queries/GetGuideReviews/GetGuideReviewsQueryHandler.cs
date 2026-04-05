using AutoMapper;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;

public class GetGuideReviewsQueryHandler : IRequestHandler<GetGuideReviewsQuery, IReadOnlyList<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetGuideReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ReviewDto>> Handle(GetGuideReviewsQuery request, CancellationToken ct)
    {
        var reviews = await _reviewRepository.GetByGuideIdAsync(request.GuideId, ct);
        return _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
    }
}
