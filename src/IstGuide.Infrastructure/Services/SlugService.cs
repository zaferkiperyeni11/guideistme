using IstGuide.Application.Common.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace IstGuide.Infrastructure.Services;

public class SlugService : ISlugService
{
    private readonly IApplicationDbContext _context;

    public SlugService(IApplicationDbContext context)
    {
        _context = context;
    }

    public string GenerateSlug(string text)
    {
        // Türkçe karakterleri dönüştür
        var normalized = text.ToLowerInvariant();
        normalized = normalized
            .Replace('ş', 's').Replace('Ş', 's')
            .Replace('ı', 'i').Replace('İ', 'i')
            .Replace('ğ', 'g').Replace('Ğ', 'g')
            .Replace('ü', 'u').Replace('Ü', 'u')
            .Replace('ö', 'o').Replace('Ö', 'o')
            .Replace('ç', 'c').Replace('Ç', 'c');

        // ASCII dışı karakterleri kaldır
        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(normalized);
        normalized = Encoding.ASCII.GetString(bytes);

        // Boşluk ve özel karakterleri tire yap
        normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");
        normalized = Regex.Replace(normalized, @"\s+", "-");
        normalized = Regex.Replace(normalized, @"-+", "-");
        normalized = normalized.Trim('-');

        return normalized;
    }

    public async Task<string> GenerateUniqueSlugAsync(string text, CancellationToken ct = default)
    {
        var baseSlug = GenerateSlug(text);
        var slug = baseSlug;
        var counter = 1;

        while (await SlugExistsAsync(slug, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    public async Task<string> GenerateUniqueSlugAsync(string text, Guid excludeId, CancellationToken ct = default)
    {
        var baseSlug = GenerateSlug(text);
        var slug = baseSlug;
        var counter = 1;

        while (await SlugExistsAsync(slug, excludeId, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
    {
        return await Task.FromResult(
            _context.Guides.Any(g => g.Slug == slug && !g.IsDeleted));
    }

    private async Task<bool> SlugExistsAsync(string slug, Guid excludeId, CancellationToken ct)
    {
        return await Task.FromResult(
            _context.Guides.Any(g => g.Slug == slug && g.Id != excludeId && !g.IsDeleted));
    }
}
