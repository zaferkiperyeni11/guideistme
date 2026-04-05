using AutoMapper;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;

public class GetApprovedGuidesQueryHandler : IRequestHandler<GetApprovedGuidesQuery, IReadOnlyList<GuideListDto>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IMapper _mapper;

    public GetApprovedGuidesQueryHandler(IGuideRepository guideRepository, IMapper mapper)
    {
        _guideRepository = guideRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GuideListDto>> Handle(GetApprovedGuidesQuery request, CancellationToken ct)
    {
        var guides = await _guideRepository.GetApprovedGuidesAsync(ct);
        return _mapper.Map<IReadOnlyList<GuideListDto>>(guides);
    }
}
