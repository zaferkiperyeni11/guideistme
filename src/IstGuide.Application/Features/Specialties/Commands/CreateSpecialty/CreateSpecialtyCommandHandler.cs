using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using MediatR;

namespace IstGuide.Application.Features.Specialties.Commands.CreateSpecialty;

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISlugService _slugService;

    public CreateSpecialtyCommandHandler(IApplicationDbContext context, ISlugService slugService)
    {
        _context = context;
        _slugService = slugService;
    }

    public async Task<Result<Guid>> Handle(CreateSpecialtyCommand request, CancellationToken ct)
    {
        var slug = await _slugService.GenerateUniqueSlugAsync(request.Name);

        var specialty = new Specialty
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive
        };

        _context.Specialties.Add(specialty);
        await _context.SaveChangesAsync(ct);

        return Result<Guid>.Success(specialty.Id);
    }
}
