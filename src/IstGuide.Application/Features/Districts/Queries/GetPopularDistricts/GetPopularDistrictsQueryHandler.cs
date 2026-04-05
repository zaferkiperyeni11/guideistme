using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Features.Districts.Queries.GetAllDistricts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Districts.Queries.GetPopularDistricts;

public class GetPopularDistrictsQueryHandler : IRequestHandler<GetPopularDistrictsQuery, IReadOnlyList<DistrictDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPopularDistrictsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DistrictDto>> Handle(GetPopularDistrictsQuery request, CancellationToken ct)
    {
        return await _context.Districts
            .Where(x => x.IsPopular && !x.IsDeleted)
            .OrderBy(x => x.SortOrder)
            .Select(x => new DistrictDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                IsPopular = x.IsPopular,
                SortOrder = x.SortOrder
            })
            .ToListAsync(ct);
    }
}
