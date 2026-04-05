using IstGuide.Application.Common.Interfaces;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly IApplicationDbContext _context;

    public ReviewRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Reviews.FindAsync(new object[] { id }, ct);

    public async Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Reviews.ToListAsync(ct);

    public async Task<Review> AddAsync(Review entity, CancellationToken ct = default)
    {
        await _context.Reviews.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(Review entity, CancellationToken ct = default)
    {
        _context.Reviews.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Review entity, CancellationToken ct = default)
    {
        _context.Reviews.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Review>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default) =>
        await _context.Reviews
            .Where(r => r.GuideId == guideId && r.Status == ReviewStatus.Approved)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Review>> GetPendingReviewsAsync(CancellationToken ct = default) =>
        await _context.Reviews
            .Where(r => r.Status == ReviewStatus.Pending)
            .Include(r => r.Guide)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<double> GetAverageRatingAsync(Guid guideId, CancellationToken ct = default)
    {
        var approved = _context.Reviews
            .Where(r => r.GuideId == guideId && r.Status == ReviewStatus.Approved);

        if (!await approved.AnyAsync(ct)) return 0;
        return await approved.AverageAsync(r => (double)r.Rating, ct);
    }
}
