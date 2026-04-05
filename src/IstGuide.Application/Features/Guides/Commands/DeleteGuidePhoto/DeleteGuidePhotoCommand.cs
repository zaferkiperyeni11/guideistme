using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Guides.Commands.DeleteGuidePhoto;

public record DeleteGuidePhotoCommand(Guid GuideId, Guid PhotoId) : IRequest<Result>;

public class DeleteGuidePhotoCommandHandler : IRequestHandler<DeleteGuidePhotoCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuidePhotoCommandHandler(IApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGuidePhotoCommand request, CancellationToken ct)
    {
        var photo = await _context.GuidePhotos
            .FirstOrDefaultAsync(x => x.Id == request.PhotoId && x.GuideId == request.GuideId, ct)
            ?? throw new NotFoundException(nameof(GuidePhoto), request.PhotoId);

        _context.GuidePhotos.Remove(photo);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
