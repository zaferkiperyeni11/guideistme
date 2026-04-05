using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Entities;
using MediatR;

namespace IstGuide.Application.Features.Districts.Commands.CreateDistrict;

public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISlugService _slugService;

    public CreateDistrictCommandHandler(IApplicationDbContext context, ISlugService slugService)
    {
        _context = context;
        _slugService = slugService;
    }

    public async Task<Result<Guid>> Handle(CreateDistrictCommand request, CancellationToken ct)
    {
        var slug = await _slugService.GenerateUniqueSlugAsync(request.Name);

        var district = new District
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            SortOrder = request.SortOrder,
            IsPopular = request.IsPopular
        };

        _context.Districts.Add(district);
        await _context.SaveChangesAsync(ct);

        return Result<Guid>.Success(district.Id);
    }
}
