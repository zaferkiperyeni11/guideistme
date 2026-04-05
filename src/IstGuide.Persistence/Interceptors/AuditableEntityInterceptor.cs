using IstGuide.Domain.Common;
using IstGuide.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IstGuide.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IstGuide.Application.Common.Interfaces.ICurrentUserService _currentUserService;

    public AuditableEntityInterceptor(IstGuide.Application.Common.Interfaces.ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var now = DateTime.UtcNow;
        var userId = _currentUserService.UserName ?? "System";

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedAt = now;
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedBy = userId;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedBy = userId;
        }
    }
}
