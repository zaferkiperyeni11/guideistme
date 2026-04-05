using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Districts.Commands.UpdateDistrict;

public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ISlugService _slugService;

    public UpdateDistrictCommandHandler(IApplicationDbContext context, ISlugService slugService)
    {
        _context = context;
        _slugService = slugService;
    }

    public async Task<Result> Handle(UpdateDistrictCommand request, CancellationToken ct)
    {
        var district = await _context.Districts
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(District), request.Id);

        district.Name = request.Name;
        district.Slug = await _slugService.GenerateUniqueSlugAsync(request.Name, request.Id);
        district.Description = request.Description;
        district.ImageUrl = request.ImageUrl;
        district.Latitude = request.Latitude;
        district.Longitude = request.Longitude;
        district.SortOrder = request.SortOrder;
        district.IsPopular = request.IsPopular;
        district.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return Result.Success();
    }
}
