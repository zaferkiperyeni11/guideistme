using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Dashboard.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<Result<DashboardStatsDto>>;

public class DashboardStatsDto
{
    public int TotalGuides { get; set; }
    public int PendingApprovals { get; set; }
    public int ApprovedGuides { get; set; }
    public int TotalReviews { get; set; }
    public int PendingReviews { get; set; }
    public int TotalContactRequests { get; set; }
    public int NewContactRequests { get; set; }
}

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardStatsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken ct)
    {
        var stats = new DashboardStatsDto
        {
            TotalGuides = await _context.Guides.CountAsync(g => !g.IsDeleted, ct),
            PendingApprovals = await _context.Guides.CountAsync(g => !g.IsDeleted && g.Status == GuideStatus.Pending, ct),
            ApprovedGuides = await _context.Guides.CountAsync(g => !g.IsDeleted && g.Status == GuideStatus.Approved, ct),
            TotalReviews = await _context.Reviews.CountAsync(r => !r.IsDeleted, ct),
            PendingReviews = await _context.Reviews.CountAsync(r => !r.IsDeleted && r.Status == ReviewStatus.Pending, ct),
            TotalContactRequests = await _context.ContactRequests.CountAsync(cr => !cr.IsDeleted, ct),
            NewContactRequests = await _context.ContactRequests.CountAsync(cr => !cr.IsDeleted && cr.Status == ContactRequestStatus.New, ct)
        };

        return Result<DashboardStatsDto>.Success(stats);
    }
}
