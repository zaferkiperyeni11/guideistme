using AutoMapper;
using IstGuide.Application.Common.Exceptions;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetGuideBySlug;

public class GetGuideBySlugQueryHandler : IRequestHandler<GetGuideBySlugQuery, GuideDetailDto>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IMapper _mapper;

    public GetGuideBySlugQueryHandler(IGuideRepository guideRepository, IMapper mapper)
    {
        _guideRepository = guideRepository;
        _mapper = mapper;
    }

    public async Task<GuideDetailDto> Handle(GetGuideBySlugQuery request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetBySlugAsync(request.Slug, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.Slug);

        return _mapper.Map<GuideDetailDto>(guide);
    }
}
