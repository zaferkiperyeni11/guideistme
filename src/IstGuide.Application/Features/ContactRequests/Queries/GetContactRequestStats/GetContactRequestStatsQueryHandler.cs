using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.ContactRequests.Queries.GetContactRequestStats;

public class GetContactRequestStatsQueryHandler : IRequestHandler<GetContactRequestStatsQuery, ContactRequestStatsDto>
{
    private readonly IApplicationDbContext _context;

    public GetContactRequestStatsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactRequestStatsDto> Handle(GetContactRequestStatsQuery request, CancellationToken ct)
    {
        var total = await _context.ContactRequests.CountAsync(ct);
        var newCount = await _context.ContactRequests.CountAsync(x => x.Status == ContactRequestStatus.New, ct);
        var viewedCount = await _context.ContactRequests.CountAsync(x => x.Status == ContactRequestStatus.Viewed, ct);
        var repliedCount = await _context.ContactRequests.CountAsync(x => x.Status == ContactRequestStatus.Replied, ct);
        var convertedCount = await _context.ContactRequests.CountAsync(x => x.Status == ContactRequestStatus.Converted, ct);
        var closedCount = await _context.ContactRequests.CountAsync(x => x.Status == ContactRequestStatus.Closed, ct);

        return new ContactRequestStatsDto
        {
            TotalRequests = total,
            NewRequests = newCount,
            ViewedRequests = viewedCount,
            RepliedRequests = repliedCount,
            ConvertedRequests = convertedCount,
            ClosedRequests = closedCount
        };
    }
}
