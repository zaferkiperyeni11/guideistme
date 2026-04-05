using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Tours.Commands.UpdateTour;

public class UpdateTourCommandHandler : IRequestHandler<UpdateTourCommand, Result>
{
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourCommandHandler(ITourRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTourCommand request, CancellationToken ct)
    {
        var tour = await _tourRepository.GetByIdAsync(request.TourId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Tour), request.TourId);

        tour.Title = request.Title;
        tour.Description = request.Description;
        tour.Price = request.Price;
        tour.Duration = request.Duration;
        tour.ImageUrl = request.ImageUrl;
        tour.DistrictId = request.DistrictId;
        tour.IsActive = request.IsActive;

        await _tourRepository.UpdateAsync(tour, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
