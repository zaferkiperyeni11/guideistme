using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Pages.Commands.UpdatePageContent;

public class UpdatePageContentCommandHandler : IRequestHandler<UpdatePageContentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdatePageContentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdatePageContentCommand request, CancellationToken ct)
    {
        var page = await _context.PageContents
            .FirstOrDefaultAsync(p => p.Key == request.Key && !p.IsDeleted, ct);

        if (page is null)
        {
            page = new PageContent { Key = request.Key };
            _context.PageContents.Add(page);
        }

        page.Title = request.Title;
        page.Content = request.Content;
        page.MetaTitle = request.MetaTitle;
        page.MetaDescription = request.MetaDescription;
        page.IsPublished = request.IsPublished;

        await _context.SaveChangesAsync(ct);

        return Result.Success();
    }
}
