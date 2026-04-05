using AutoMapper;
using IstGuide.Application.Common.Models;
using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.SearchGuides;

public class SearchGuidesQueryHandler : IRequestHandler<SearchGuidesQuery, PaginatedList<GuideListDto>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IMapper _mapper;

    public SearchGuidesQueryHandler(IGuideRepository guideRepository, IMapper mapper)
    {
        _guideRepository = guideRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GuideListDto>> Handle(SearchGuidesQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await _guideRepository.SearchGuidesAsync(request.Criteria, ct);
        var dtos = _mapper.Map<IReadOnlyList<GuideListDto>>(items);
        return new PaginatedList<GuideListDto>(dtos, totalCount, request.Criteria.Page, request.Criteria.PageSize);
    }
}
