using IstGuide.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.ContactRequests.Queries.GetContactRequests;

public class GetContactRequestsQueryHandler : IRequestHandler<GetContactRequestsQuery, IReadOnlyList<ContactRequestDto>>
{
    private readonly IApplicationDbContext _context;

    public GetContactRequestsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ContactRequestDto>> Handle(GetContactRequestsQuery request, CancellationToken ct)
    {
        var query = _context.ContactRequests
            .Include(cr => cr.Guide)
            .Where(cr => !cr.IsDeleted);

        if (request.Status.HasValue)
            query = query.Where(cr => cr.Status == request.Status.Value);

        if (request.GuideId.HasValue)
            query = query.Where(cr => cr.GuideId == request.GuideId.Value);

        var items = await query.OrderByDescending(cr => cr.CreatedAt).ToListAsync(ct);

        return items.Select(cr => new ContactRequestDto
        {
            Id = cr.Id,
            GuideId = cr.GuideId,
            GuideName = $"{cr.Guide.FirstName} {cr.Guide.LastName}",
            VisitorName = cr.VisitorName,
            VisitorEmail = cr.VisitorEmail,
            VisitorPhone = cr.VisitorPhone,
            Message = cr.Message,
            PreferredDate = cr.PreferredDate,
            GroupSize = cr.GroupSize,
            Status = cr.Status,
            Source = cr.Source,
            CreatedAt = cr.CreatedAt,
            AdminNotes = cr.AdminNotes
        }).ToList();
    }
}
