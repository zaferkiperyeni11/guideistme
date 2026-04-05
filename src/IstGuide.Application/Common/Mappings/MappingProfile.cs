using AutoMapper;
using IstGuide.Application.Features.Guides.Queries.GetGuideBySlug;
using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Application.Features.Guides.Queries.GetAllGuidesAdmin;
using IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;
using IstGuide.Domain.Entities;

namespace IstGuide.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Guide, GuideListDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.Languages, o => o.MapFrom(s => s.Languages.Select(l => l.Language.Name).ToList()))
            .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialties.Select(sp => sp.Specialty.Name).ToList()))
            .ForMember(d => d.Districts, o => o.MapFrom(s => s.ServiceDistricts.Select(d => d.District.Name).ToList()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.WhatsAppUrl, o => o.MapFrom(s => s.PhoneNumber != null ? $"https://wa.me/{s.PhoneNumber.Replace("+", "")}" : null));

        CreateMap<Guide, GuideAdminDto>()
            .IncludeBase<Guide, GuideListDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt));

        CreateMap<Guide, GuideDetailDto>()
            .IncludeBase<Guide, GuideListDto>()
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos))
            .ForMember(d => d.Certificates, o => o.MapFrom(s => s.Certificates))
            .ForMember(d => d.Availabilities, o => o.MapFrom(s => s.Availabilities));

        CreateMap<GuidePhoto, GuidePhotoDto>();
        CreateMap<GuideCertificate, GuideCertificateDto>();
        CreateMap<GuideAvailability, GuideAvailabilityDto>();
        CreateMap<Review, ReviewDto>()
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt));
    }
}