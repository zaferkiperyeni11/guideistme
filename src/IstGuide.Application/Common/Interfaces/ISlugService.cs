namespace IstGuide.Application.Common.Interfaces;

public interface ISlugService
{
    string GenerateSlug(string input);
    Task<string> GenerateUniqueSlugAsync(string input, CancellationToken ct = default);
    Task<string> GenerateUniqueSlugAsync(string input, Guid excludeId, CancellationToken ct = default);
}
