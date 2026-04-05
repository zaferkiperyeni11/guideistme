using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Events;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.RejectGuide;

public class RejectGuideCommandHandler : IRequestHandler<RejectGuideCommand, Result>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectGuideCommandHandler(IGuideRepository guideRepository, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RejectGuideCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        guide.Status = GuideStatus.Rejected;
        guide.AddDomainEvent(new GuideRejectedEvent(guide.Id, request.Reason));

        await _guideRepository.UpdateAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
