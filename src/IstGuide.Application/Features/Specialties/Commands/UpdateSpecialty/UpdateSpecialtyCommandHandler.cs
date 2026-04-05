using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Specialties.Commands.UpdateSpecialty;

public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ISlugService _slugService;

    public UpdateSpecialtyCommandHandler(IApplicationDbContext context, ISlugService slugService)
    {
        _context = context;
        _slugService = slugService;
    }

    public async Task<Result> Handle(UpdateSpecialtyCommand request, CancellationToken ct)
    {
        var specialty = await _context.Specialties
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Specialty), request.Id);

        specialty.Name = request.Name;
        specialty.Slug = await _slugService.GenerateUniqueSlugAsync(request.Name, request.Id);
        specialty.Description = request.Description;
        specialty.IconUrl = request.IconUrl;
        specialty.SortOrder = request.SortOrder;
        specialty.IsActive = request.IsActive;
        specialty.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return Result.Success();
    }
}
