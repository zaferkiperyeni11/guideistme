using IstGuide.Application.Common.Interfaces;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Persistence.Repositories;

public class GuideRepository : IGuideRepository
{
    private readonly IApplicationDbContext _context;

    public GuideRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guide?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Guides
            .Include(g => g.Languages).ThenInclude(gl => gl.Language)
            .Include(g => g.Specialties).ThenInclude(gs => gs.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(gd => gd.District)
            .Include(g => g.Photos)
            .Include(g => g.Certificates)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task<IReadOnlyList<Guide>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Guides.ToListAsync(ct);

    public async Task<Guide> AddAsync(Guide entity, CancellationToken ct = default)
    {
        await _context.Guides.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(Guide entity, CancellationToken ct = default)
    {
        _context.Guides.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guide entity, CancellationToken ct = default)
    {
        _context.Guides.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<Guide?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        await _context.Guides
            .Include(g => g.Languages).ThenInclude(gl => gl.Language)
            .Include(g => g.Specialties).ThenInclude(gs => gs.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(gd => gd.District)
            .Include(g => g.Photos)
            .Include(g => g.Certificates)
            .Include(g => g.Availabilities)
            .Include(g => g.Reviews.Where(r => r.Status == ReviewStatus.Approved).OrderByDescending(r => r.CreatedAt).Take(5))
            .FirstOrDefaultAsync(g => g.Slug == slug, ct);

    public async Task<Guide?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Guides.FirstOrDefaultAsync(g => g.Email == email, ct);

    public async Task<IReadOnlyList<Guide>> GetApprovedGuidesAsync(CancellationToken ct = default) =>
        await _context.Guides
            .Where(g => g.Status == GuideStatus.Approved)
            .Include(g => g.Languages).ThenInclude(gl => gl.Language)
            .Include(g => g.Specialties).ThenInclude(gs => gs.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(gd => gd.District)
            .OrderByDescending(g => g.AverageRating)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Guide>> GetFeaturedGuidesAsync(CancellationToken ct = default) =>
        await _context.Guides
            .Where(g => g.Status == GuideStatus.Approved && g.IsFeatured)
            .Include(g => g.Languages).ThenInclude(gl => gl.Language)
            .Include(g => g.Specialties).ThenInclude(gs => gs.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(gd => gd.District)
            .OrderByDescending(g => g.AverageRating)
            .ToListAsync(ct);

    public async Task<(IReadOnlyList<Guide> Items, int TotalCount)> SearchGuidesAsync(
        GuideSearchCriteria criteria, CancellationToken ct = default)
    {
        var query = _context.Guides
            .Where(g => g.Status == GuideStatus.Approved)
            .Include(g => g.Languages).ThenInclude(gl => gl.Language)
            .Include(g => g.Specialties).ThenInclude(gs => gs.Specialty)
            .Include(g => g.ServiceDistricts).ThenInclude(gd => gd.District)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            var term = criteria.SearchTerm.ToLower();
            query = query.Where(g =>
                g.FirstName.ToLower().Contains(term) ||
                g.LastName.ToLower().Contains(term) ||
                g.Bio.ToLower().Contains(term) ||
                g.Title.ToLower().Contains(term));
        }

        if (criteria.DistrictId.HasValue)
            query = query.Where(g => g.ServiceDistricts.Any(gd => gd.DistrictId == criteria.DistrictId));

        if (criteria.SpecialtyId.HasValue)
            query = query.Where(g => g.Specialties.Any(gs => gs.SpecialtyId == criteria.SpecialtyId));

        if (criteria.LanguageId.HasValue)
            query = query.Where(g => g.Languages.Any(gl => gl.LanguageId == criteria.LanguageId));

        if (criteria.MinRating.HasValue)
            query = query.Where(g => g.AverageRating >= criteria.MinRating);

        var totalCount = await query.CountAsync(ct);

        query = criteria.SortBy switch
        {
            "experience" => query.OrderByDescending(g => g.YearsOfExperience),
            "reviews"    => query.OrderByDescending(g => g.ReviewCount),
            _            => query.OrderByDescending(g => g.AverageRating)
        };

        var items = await query
            .Skip((criteria.Page - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default) =>
        await _context.Guides.AnyAsync(g => g.Slug == slug, ct);
}
