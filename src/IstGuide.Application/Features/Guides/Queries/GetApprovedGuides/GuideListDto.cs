namespace IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;

public class GuideListDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Bio { get; set; } = default!;
    public string? ProfilePhotoUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<string> Specialties { get; set; } = new();
    public List<string> Districts { get; set; } = new();
    public bool IsFeatured { get; set; }
    public string? WhatsAppUrl { get; set; }
    public string Status { get; set; } = default!;
}
