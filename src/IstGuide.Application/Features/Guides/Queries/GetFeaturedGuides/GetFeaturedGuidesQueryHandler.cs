using AutoMapper;
using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetFeaturedGuides;

public class GetFeaturedGuidesQueryHandler : IRequestHandler<GetFeaturedGuidesQuery, IReadOnlyList<GuideListDto>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IMapper _mapper;

    public GetFeaturedGuidesQueryHandler(IGuideRepository guideRepository, IMapper mapper)
    {
        _guideRepository = guideRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GuideListDto>> Handle(GetFeaturedGuidesQuery request, CancellationToken ct)
    {
        var guides = await _guideRepository.GetFeaturedGuidesAsync(ct);
        return _mapper.Map<IReadOnlyList<GuideListDto>>(guides);
    }
}
