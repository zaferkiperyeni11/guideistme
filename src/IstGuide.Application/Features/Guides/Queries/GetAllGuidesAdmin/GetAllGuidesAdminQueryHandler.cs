using AutoMapper;
using IstGuide.Application.Common.Interfaces;
using IstGuide.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Guides.Queries.GetAllGuidesAdmin;

public class GetAllGuidesAdminQueryHandler : IRequestHandler<GetAllGuidesAdminQuery, IReadOnlyList<GuideAdminDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllGuidesAdminQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GuideAdminDto>> Handle(GetAllGuidesAdminQuery request, CancellationToken ct)
    {
        var query = _context.Guides
            .Include(g => g.Languages).ThenInclude(l => l.Language)
            .Include(g => g.Specialties).ThenInclude(s => s.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(d => d.District)
            .Where(g => !g.IsDeleted);

        if (request.Status.HasValue)
            query = query.Where(g => g.Status == request.Status.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(g => g.FirstName.ToLower().Contains(term)
                || g.LastName.ToLower().Contains(term)
                || g.Email.ToLower().Contains(term));
        }

        var guides = await query.OrderByDescending(g => g.CreatedAt).ToListAsync(ct);
        return _mapper.Map<IReadOnlyList<GuideAdminDto>>(guides);
    }
}
