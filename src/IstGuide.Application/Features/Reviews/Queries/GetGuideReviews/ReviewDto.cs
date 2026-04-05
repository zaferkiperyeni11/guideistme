using IstGuide.Domain.Enums;

namespace IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string ReviewerName { get; set; } = default!;
    public string? ReviewerCountry { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? AdminReply { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
