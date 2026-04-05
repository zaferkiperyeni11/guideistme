using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;

namespace IstGuide.Domain.Repositories;

public class GuideSearchCriteria
{
    public string? SearchTerm { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? SpecialtyId { get; set; }
    public Guid? LanguageId { get; set; }
    public double? MinRating { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

public interface IGuideRepository : IRepository<Guide>
{
    Task<Guide?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Guide?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<Guide>> GetApprovedGuidesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Guide>> GetFeaturedGuidesAsync(CancellationToken ct = default);
    Task<(IReadOnlyList<Guide> Items, int TotalCount)> SearchGuidesAsync(GuideSearchCriteria criteria, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);
}
