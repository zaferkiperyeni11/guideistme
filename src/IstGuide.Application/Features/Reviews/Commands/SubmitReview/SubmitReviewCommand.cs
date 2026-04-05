using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Events;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.SubmitReview;

public record SubmitReviewCommand(Guid GuideId, string ReviewerName, string? ReviewerEmail, int Rating, string? Comment) : IRequest<Result<Guid>>;

public class SubmitReviewCommandHandler : IRequestHandler<SubmitReviewCommand, Result<Guid>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitReviewCommandHandler(IReviewRepository reviewRepository, IGuideRepository guideRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(SubmitReviewCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct);
        if (guide == null)
            return Result<Guid>.Failure("Rehber bulunamadı.");

        var review = new Review
        {
            GuideId = request.GuideId,
            ReviewerName = request.ReviewerName,
            ReviewerEmail = request.ReviewerEmail,
            Rating = request.Rating,
            Comment = request.Comment,
            Status = ReviewStatus.Pending // Onaylanmayı bekler
        };

        review.AddDomainEvent(new ReviewSubmittedEvent(review.Id, guide.Id));

        await _reviewRepository.AddAsync(review, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(review.Id);
    }
}
