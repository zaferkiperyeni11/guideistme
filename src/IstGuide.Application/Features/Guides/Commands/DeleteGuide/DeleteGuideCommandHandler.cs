using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.DeleteGuide;

public class DeleteGuideCommandHandler : IRequestHandler<DeleteGuideCommand, Result>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuideCommandHandler(IGuideRepository guideRepository, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGuideCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        guide.IsDeleted = true;
        await _guideRepository.UpdateAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
