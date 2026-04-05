using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Enums;
using IstGuide.Domain.Events;
using IstGuide.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.Guides.Commands.RegisterGuide;

public class RegisterGuideCommandHandler : IRequestHandler<RegisterGuideCommand, Result<Guid>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly ISlugService _slugService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _context;

    public RegisterGuideCommandHandler(
        IGuideRepository guideRepository,
        ISlugService slugService,
        IUnitOfWork unitOfWork,
        IApplicationDbContext context)
    {
        _guideRepository = guideRepository;
        _slugService = slugService;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<Guid>> Handle(RegisterGuideCommand request, CancellationToken ct)
    {
        var existing = await _guideRepository.GetByEmailAsync(request.Email, ct);
        if (existing != null)
            return Result<Guid>.Failure("Bu email adresi zaten kayıtlı.");

        var slug = await _slugService.GenerateUniqueSlugAsync(
            $"{request.FirstName}-{request.LastName}-istanbul", ct);

        var guide = new Guide
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            WhatsAppUrl = $"https://wa.me/90{request.PhoneNumber.TrimStart('0')}",
            Slug = slug,
            Title = request.Title,
            Bio = request.Bio,
            DetailedDescription = request.DetailedDescription,
            YearsOfExperience = request.YearsOfExperience,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            LicenseNumber = request.LicenseNumber,
            Status = GuideStatus.Pending
        };

        if (request.LanguageIds != null)
        {
            foreach (var langId in request.LanguageIds)
                guide.Languages.Add(new GuideLanguage { LanguageId = langId });
        }

        if (request.SpecialtyIds != null)
        {
            foreach (var specId in request.SpecialtyIds)
                guide.Specialties.Add(new GuideSpecialty { SpecialtyId = specId });
        }

        if (request.DistrictIds != null)
        {
            foreach (var distId in request.DistrictIds)
                guide.ServiceDistricts.Add(new GuideDistrict { DistrictId = distId });
        }

        guide.AddDomainEvent(new GuideRegisteredEvent(guide.Id));

        await _guideRepository.AddAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(guide.Id);
    }
}
