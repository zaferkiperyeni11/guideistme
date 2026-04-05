using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;

namespace IstGuide.Domain.Repositories;

public interface IContactRequestRepository : IRepository<ContactRequest>
{
    Task<IReadOnlyList<ContactRequest>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default);
    Task<int> GetNewRequestCountAsync(CancellationToken ct = default);
}
