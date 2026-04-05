using IstGuide.Domain.Entities;

namespace IstGuide.Domain.Repositories;

public interface ITourRepository
{
    Task<Tour?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Tour?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<IEnumerable<Tour>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default);
    Task<IEnumerable<Tour>> GetFeaturedToursAsync(CancellationToken ct = default);
    Task<IEnumerable<Tour>> GetActiveToursAsync(CancellationToken ct = default);
    Task AddAsync(Tour tour, CancellationToken ct = default);
    Task UpdateAsync(Tour tour, CancellationToken ct = default);
}
