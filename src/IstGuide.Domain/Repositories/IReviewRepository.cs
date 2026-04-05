using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;

namespace IstGuide.Domain.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<IReadOnlyList<Review>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default);
    Task<IReadOnlyList<Review>> GetPendingReviewsAsync(CancellationToken ct = default);
    Task<double> GetAverageRatingAsync(Guid guideId, CancellationToken ct = default);
}
