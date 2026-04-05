using IstGuide.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Specialties.Queries.GetAllSpecialties;

public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, IReadOnlyList<SpecialtyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSpecialtiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SpecialtyDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken ct)
    {
        return await _context.Specialties
            .Where(s => !s.IsDeleted && s.IsActive)
            .OrderBy(s => s.SortOrder)
            .Select(s => new SpecialtyDto
            {
                Id = s.Id,
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description,
                IconUrl = s.IconUrl,
                SortOrder = s.SortOrder
            })
            .ToListAsync(ct);
    }
}
