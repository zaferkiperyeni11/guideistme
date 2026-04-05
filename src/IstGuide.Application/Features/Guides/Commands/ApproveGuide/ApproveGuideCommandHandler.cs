using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Events;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.ApproveGuide;

public class ApproveGuideCommandHandler : IRequestHandler<ApproveGuideCommand, Result>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveGuideCommandHandler(IGuideRepository guideRepository, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ApproveGuideCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        guide.Status = GuideStatus.Approved;
        guide.AddDomainEvent(new GuideApprovedEvent(guide.Id));

        await _guideRepository.UpdateAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
