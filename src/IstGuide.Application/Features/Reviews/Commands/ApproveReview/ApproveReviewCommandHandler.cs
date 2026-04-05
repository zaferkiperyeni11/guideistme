using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.ApproveReview;

public class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveReviewCommandHandler(
        IReviewRepository reviewRepository,
        IGuideRepository guideRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ApproveReviewCommand request, CancellationToken ct)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Review), request.ReviewId);

        review.Status = ReviewStatus.Approved;

        // Update guide's average rating
        var guide = await _guideRepository.GetByIdAsync(review.GuideId, ct);
        if (guide != null)
        {
            var avg = await _reviewRepository.GetAverageRatingAsync(guide.Id, ct);
            guide.AverageRating = avg;
            guide.ReviewCount = (await _reviewRepository.GetByGuideIdAsync(guide.Id, ct)).Count;
            await _guideRepository.UpdateAsync(guide, ct);
        }

        await _reviewRepository.UpdateAsync(review, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
