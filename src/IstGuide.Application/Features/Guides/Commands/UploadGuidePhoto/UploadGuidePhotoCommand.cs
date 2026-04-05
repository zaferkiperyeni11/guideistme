using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.UploadGuidePhoto;

public record UploadGuidePhotoCommand(Guid GuideId, Stream FileStream, string FileName, bool IsPrimary) : IRequest<Result<string>>;

public class UploadGuidePhotoCommandHandler : IRequestHandler<UploadGuidePhotoCommand, Result<string>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public UploadGuidePhotoCommandHandler(IGuideRepository guideRepository, IFileStorageService fileStorageService, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(UploadGuidePhotoCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct);
        if (guide == null)
            return Result<string>.Failure("Rehber bulunamadı.");

        var fileUrl = await _fileStorageService.UploadFileAsync(request.FileStream, request.FileName, "image/jpeg", ct);

        var photo = new GuidePhoto
        {
            GuideId = guide.Id,
            Url = fileUrl,
            IsPrimary = request.IsPrimary
        };

        guide.Photos.Add(photo);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<string>.Success(fileUrl);
    }
}
