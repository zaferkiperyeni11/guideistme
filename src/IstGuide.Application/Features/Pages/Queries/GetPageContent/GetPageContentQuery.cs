using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Pages.Queries.GetPageContent;

public record GetPageContentQuery(string Key) : IRequest<Result<PageContentDto>>;

public class PageContentDto
{
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}

public class GetPageContentQueryHandler : IRequestHandler<GetPageContentQuery, Result<PageContentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPageContentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PageContentDto>> Handle(GetPageContentQuery request, CancellationToken ct)
    {
        var page = await _context.PageContents
            .Where(p => p.Key == request.Key && p.IsPublished && !p.IsDeleted)
            .Select(p => new PageContentDto
            {
                Key = p.Key,
                Title = p.Title,
                Content = p.Content,
                MetaTitle = p.MetaTitle,
                MetaDescription = p.MetaDescription
            })
            .FirstOrDefaultAsync(ct);

        if (page == null)
            return Result<PageContentDto>.Failure($"Sayfa bulunamadı: {request.Key}");

        return Result<PageContentDto>.Success(page);
    }
}
