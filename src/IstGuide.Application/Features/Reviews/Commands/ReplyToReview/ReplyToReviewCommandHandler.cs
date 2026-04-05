using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.ReplyToReview;

public class ReplyToReviewCommandHandler : IRequestHandler<ReplyToReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplyToReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReplyToReviewCommand request, CancellationToken ct)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Review), request.ReviewId);

        review.AdminReply = request.Reply;

        await _reviewRepository.UpdateAsync(review, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
