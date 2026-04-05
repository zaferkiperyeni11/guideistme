using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Pages.Commands.UpdatePageContent;

public record UpdatePageContentCommand : IRequest<Result>
{
    public string Key { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public bool IsPublished { get; init; }
}
