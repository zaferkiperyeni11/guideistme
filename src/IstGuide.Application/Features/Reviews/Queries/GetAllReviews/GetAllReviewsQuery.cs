using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Reviews.Queries.GetAllReviews;

public record GetAllReviewsQuery : IRequest<Result<List<ReviewDto>>>
{
    public Domain.Enums.ReviewStatus? Status { get; init; }
}

public record ReviewDto
{
    public Guid Id { get; set; }
    public Guid GuideId { get; set; }
    public string GuideName { get; set; } = default!;
    public string ReviewerName { get; set; } = default!;
    public string? ReviewerEmail { get; set; }
    public string? ReviewerCountry { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? AdminReply { get; set; }
    public Domain.Enums.ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
