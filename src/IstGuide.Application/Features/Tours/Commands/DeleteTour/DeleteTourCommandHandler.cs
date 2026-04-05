using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Tours.Commands.DeleteTour;

public class DeleteTourCommandHandler : IRequestHandler<DeleteTourCommand, Result>
{
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTourCommandHandler(ITourRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTourCommand request, CancellationToken ct)
    {
        var tour = await _tourRepository.GetByIdAsync(request.TourId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Tour), request.TourId);

        tour.IsDeleted = true;
        await _tourRepository.UpdateAsync(tour, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
