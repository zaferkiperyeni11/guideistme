using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Guides.Commands.UpdateGuideProfile;

public class UpdateGuideProfileCommandHandler : IRequestHandler<UpdateGuideProfileCommand, Result>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _context;

    public UpdateGuideProfileCommandHandler(
        IGuideRepository guideRepository,
        IUnitOfWork unitOfWork,
        IApplicationDbContext context)
    {
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result> Handle(UpdateGuideProfileCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        guide.Title = request.Title;
        guide.Bio = request.Bio;
        guide.DetailedDescription = request.DetailedDescription;
        guide.YearsOfExperience = request.YearsOfExperience;
        guide.LicenseNumber = request.LicenseNumber;

        // Replace languages
        var existingLangs = await _context.GuideLanguages.Where(gl => gl.GuideId == guide.Id).ToListAsync(ct);
        _context.GuideLanguages.RemoveRange(existingLangs);
        foreach (var langId in request.LanguageIds)
            _context.GuideLanguages.Add(new GuideLanguage { GuideId = guide.Id, LanguageId = langId });

        // Replace specialties
        var existingSpecs = await _context.GuideSpecialties.Where(gs => gs.GuideId == guide.Id).ToListAsync(ct);
        _context.GuideSpecialties.RemoveRange(existingSpecs);
        foreach (var specId in request.SpecialtyIds)
            _context.GuideSpecialties.Add(new GuideSpecialty { GuideId = guide.Id, SpecialtyId = specId });

        // Replace districts
        var existingDists = await _context.GuideDistricts.Where(gd => gd.GuideId == guide.Id).ToListAsync(ct);
        _context.GuideDistricts.RemoveRange(existingDists);
        foreach (var distId in request.DistrictIds)
            _context.GuideDistricts.Add(new GuideDistrict { GuideId = guide.Id, DistrictId = distId });

        await _guideRepository.UpdateAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
