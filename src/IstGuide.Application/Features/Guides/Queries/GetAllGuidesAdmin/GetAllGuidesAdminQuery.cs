using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Domain.Enums;
using MediatR;

namespace IstGuide.Application.Features.Guides.Queries.GetAllGuidesAdmin;

public record GetAllGuidesAdminQuery : IRequest<IReadOnlyList<GuideAdminDto>>
{
    public GuideStatus? Status { get; init; }
    public string? SearchTerm { get; init; }
}

public class GuideAdminDto : GuideListDto
{
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public GuideStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProfileViewCount { get; set; }
}
