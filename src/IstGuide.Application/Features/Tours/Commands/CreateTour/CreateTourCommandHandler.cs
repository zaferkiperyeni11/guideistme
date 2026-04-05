using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Tours.Commands.CreateTour;

public class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, Result<Guid>>
{
    private readonly ITourRepository _tourRepository;
    private readonly IGuideRepository _guideRepository;
    private readonly ISlugService _slugService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTourCommandHandler(
        ITourRepository tourRepository,
        IGuideRepository guideRepository,
        ISlugService slugService,
        IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _guideRepository = guideRepository;
        _slugService = slugService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTourCommand request, CancellationToken ct)
    {
        // Rehber var mı kontrol et
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct);
        if (guide == null)
            return Result<Guid>.Failure("Rehber bulunamadı.");

        // Slug üret
        var slug = await _slugService.GenerateUniqueSlugAsync(
            $"{request.Title.ToLower().Replace(" ", "-")}", ct);

        // Tour oluştur
        var tour = new Tour
        {
            GuideId = request.GuideId,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Duration = request.Duration,
            ImageUrl = request.ImageUrl,
            Slug = slug,
            DistrictId = request.DistrictId,
            IsActive = true
        };

        await _tourRepository.AddAsync(tour, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(tour.Id);
    }
}
