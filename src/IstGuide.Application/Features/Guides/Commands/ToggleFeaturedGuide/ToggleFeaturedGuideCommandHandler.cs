using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.ToggleFeaturedGuide;

public class ToggleFeaturedGuideCommandHandler : IRequestHandler<ToggleFeaturedGuideCommand, Result<bool>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleFeaturedGuideCommandHandler(IGuideRepository guideRepository, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ToggleFeaturedGuideCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        guide.IsFeatured = !guide.IsFeatured;

        await _guideRepository.UpdateAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<bool>.Success(guide.IsFeatured);
    }
}
