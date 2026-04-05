using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.ApproveReview;

public record ApproveReviewCommand(Guid ReviewId) : IRequest<Result>;
