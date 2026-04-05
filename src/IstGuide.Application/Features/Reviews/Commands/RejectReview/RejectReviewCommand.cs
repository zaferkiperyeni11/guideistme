using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.RejectReview;

public record RejectReviewCommand(Guid ReviewId) : IRequest<Result>;
