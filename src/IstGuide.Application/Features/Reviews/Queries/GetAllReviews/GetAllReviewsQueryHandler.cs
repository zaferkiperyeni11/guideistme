using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Reviews.Queries.GetAllReviews;

public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, Result<List<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ReviewDto>>> Handle(GetAllReviewsQuery request, CancellationToken ct)
    {
        var query = _context.Reviews
            .Include(x => x.Guide)
            .Where(x => !x.IsDeleted);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        var reviews = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ReviewDto
            {
                Id = x.Id,
                GuideId = x.GuideId,
                GuideName = x.Guide.FirstName + " " + x.Guide.LastName,
                ReviewerName = x.ReviewerName,
                ReviewerEmail = x.ReviewerEmail,
                ReviewerCountry = x.ReviewerCountry,
                Rating = x.Rating,
                Comment = x.Comment,
                AdminReply = x.AdminReply,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<ReviewDto>>.Success(reviews);
    }
}
