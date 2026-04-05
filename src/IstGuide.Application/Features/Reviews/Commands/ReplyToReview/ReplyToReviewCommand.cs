using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Commands.ReplyToReview;

public record ReplyToReviewCommand(Guid ReviewId, string Reply) : IRequest<Result>;
