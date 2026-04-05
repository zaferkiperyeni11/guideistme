using IstGuide.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Languages.Queries.GetAllLanguages;

public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, IReadOnlyList<LanguageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllLanguagesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<LanguageDto>> Handle(GetAllLanguagesQuery request, CancellationToken ct)
    {
        return await _context.Languages
            .Where(l => !l.IsDeleted)
            .OrderBy(l => l.Name)
            .Select(l => new LanguageDto
            {
                Id = l.Id,
                Name = l.Name,
                Code = l.Code,
                NativeName = l.NativeName,
                FlagIconUrl = l.FlagIconUrl
            })
            .ToListAsync(ct);
    }
}
