using IstGuide.Application.Common.Interfaces;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Persistence.Repositories;

public class ContactRequestRepository : IContactRequestRepository
{
    private readonly IApplicationDbContext _context;

    public ContactRequestRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactRequest?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.ContactRequests
            .Include(c => c.Guide)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<ContactRequest>> GetAllAsync(CancellationToken ct = default) =>
        await _context.ContactRequests
            .Include(c => c.Guide)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<ContactRequest> AddAsync(ContactRequest entity, CancellationToken ct = default)
    {
        await _context.ContactRequests.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(ContactRequest entity, CancellationToken ct = default)
    {
        _context.ContactRequests.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ContactRequest entity, CancellationToken ct = default)
    {
        _context.ContactRequests.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<ContactRequest>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default) =>
        await _context.ContactRequests
            .Where(c => c.GuideId == guideId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<int> GetNewRequestCountAsync(CancellationToken ct = default) =>
        await _context.ContactRequests
            .CountAsync(c => c.Status == ContactRequestStatus.New, ct);
}
