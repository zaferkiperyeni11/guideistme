using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.RejectReview;

public class RejectReviewCommandHandler : IRequestHandler<RejectReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RejectReviewCommand request, CancellationToken ct)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Review), request.ReviewId);

        review.Status = ReviewStatus.Rejected;

        await _reviewRepository.UpdateAsync(review, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
