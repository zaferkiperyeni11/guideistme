using IstGuide.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Districts.Queries.GetAllDistricts;

public class GetAllDistrictsQueryHandler : IRequestHandler<GetAllDistrictsQuery, IReadOnlyList<DistrictDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllDistrictsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DistrictDto>> Handle(GetAllDistrictsQuery request, CancellationToken ct)
    {
        var query = _context.Districts.Where(d => !d.IsDeleted);

        if (request.PopularOnly == true)
            query = query.Where(d => d.IsPopular);

        return await query
            .OrderBy(d => d.SortOrder)
            .Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,
                Slug = d.Slug,
                Description = d.Description,
                ImageUrl = d.ImageUrl,
                IsPopular = d.IsPopular,
                SortOrder = d.SortOrder
            })
            .ToListAsync(ct);
    }
}
