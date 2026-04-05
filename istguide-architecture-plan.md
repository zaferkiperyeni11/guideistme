# IstGuide.me — Mimari Plan & Teknik Doküman

> **Domain:** istguide.me  
> **Kapsam:** İstanbul özelinde turist rehber marketplace  
> **Faz 1:** Ödeme yok, WhatsApp iletişim  
> **Hedef:** Ölçeklenebilir Clean Architecture, MSSQL Express  

---

## 1. SOLUTION MİMARİSİ (Clean Architecture)

```
IstGuide.sln
│
├── src/
│   ├── IstGuide.Domain              ← Entity'ler, Value Objects, Enum'lar, Interface'ler
│   ├── IstGuide.Application         ← CQRS, MediatR, DTO'lar, Validation, Business Rules
│   ├── IstGuide.Infrastructure      ← EF Core, Repository impl, Email, SMS, File Storage
│   ├── IstGuide.Persistence         ← DbContext, Migrations, Seed Data, Configurations
│   ├── IstGuide.API                 ← Controllers, Middleware, Filters, Program.cs
│   └── IstGuide.WebUI               ← Razor Pages / React frontend (public site)
│
├── src/admin/
│   └── IstGuide.AdminPanel          ← Admin panel (Blazor Server veya ayrı React app)
│
├── tests/
│   ├── IstGuide.Domain.Tests
│   ├── IstGuide.Application.Tests
│   └── IstGuide.API.Tests
│
└── docs/
    └── architecture.md
```

### Katman Bağımlılık Kuralı (Dependency Rule)

```
Domain ← Application ← Infrastructure / Persistence ← API / WebUI
         (hiçbir şeye             (Domain +                (her şeye
          bağlı değil)            Application'a bağlı)      bağlı)
```

Domain katmanı HİÇBİR harici NuGet paketine bağımlı olmamalı. Application katmanı sadece MediatR ve FluentValidation kullanır. Tüm altyapı detayları (EF Core, dosya sistemi, WhatsApp API) Infrastructure'da kalır.

---

## 2. DOMAIN KATMANI (IstGuide.Domain)

### 2.1 Core Base Classes

```
Domain/
├── Common/
│   ├── BaseEntity.cs                ← Id, CreatedAt, UpdatedAt, IsDeleted (soft delete)
│   ├── BaseAuditableEntity.cs       ← + CreatedBy, UpdatedBy
│   ├── IAggregateRoot.cs            ← Marker interface
│   ├── IDomainEvent.cs              ← Domain event interface
│   ├── ValueObject.cs               ← Abstract, Equals by value
│   └── IRepository<T>.cs            ← Generic repository interface
├── Entities/
├── ValueObjects/
├── Enums/
├── Events/
└── Exceptions/
```

### 2.2 BaseEntity.cs

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
```

### 2.3 BaseAuditableEntity.cs

```csharp
public abstract class BaseAuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
```

### 2.4 Entity'ler

#### Guide (Rehber)

```csharp
public class Guide : BaseAuditableEntity, IAggregateRoot
{
    // Kişisel Bilgiler
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }           // WhatsApp numarası
    public string? WhatsAppUrl { get; set; }           // Hesaplanan: https://wa.me/90xxx
    public string? ProfilePhotoUrl { get; set; }
    public string? CoverPhotoUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    // Profesyonel Bilgiler
    public string Slug { get; set; }                   // URL-friendly: "ali-yilmaz-istanbul"
    public string Title { get; set; }                  // "Lisanslı Turist Rehberi"
    public string Bio { get; set; }                    // Kısa tanıtım (max 500 karakter)
    public string? DetailedDescription { get; set; }   // Uzun açıklama
    public string? LicenseNumber { get; set; }         // TUREB ruhsat no
    public int YearsOfExperience { get; set; }
    public decimal? HourlyRate { get; set; }           // Faz 2 için
    public decimal? DailyRate { get; set; }            // Faz 2 için

    // Durum
    public GuideStatus Status { get; set; }            // Pending, Approved, Rejected, Suspended
    public bool IsFeatured { get; set; }               // Ana sayfada öne çıkarılan
    public int ProfileViewCount { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }

    // İlişkiler
    public ICollection<GuideLanguage> Languages { get; set; }
    public ICollection<GuideSpecialty> Specialties { get; set; }
    public ICollection<GuideDistrict> ServiceDistricts { get; set; }
    public ICollection<GuideCertificate> Certificates { get; set; }
    public ICollection<GuidePhoto> Photos { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<GuideAvailability> Availabilities { get; set; }

    // Application User bağlantısı (Identity)
    public string? UserId { get; set; }
}
```

#### Language (Dil)

```csharp
public class Language : BaseEntity
{
    public string Name { get; set; }         // "İngilizce"
    public string Code { get; set; }         // "en"
    public string NativeName { get; set; }   // "English"
    public string? FlagIconUrl { get; set; }

    public ICollection<GuideLanguage> GuideLanguages { get; set; }
}
```

#### GuideLanguage (Rehber-Dil Ara Tablo)

```csharp
public class GuideLanguage : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public LanguageProficiency Proficiency { get; set; } // Native, Fluent, Intermediate
}
```

#### Specialty (Uzmanlık Alanı)

```csharp
public class Specialty : BaseEntity
{
    public string Name { get; set; }          // "Tarihi Turlar"
    public string Slug { get; set; }          // "tarihi-turlar"
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public ICollection<GuideSpecialty> GuideSpecialties { get; set; }
}
```

#### GuideSpecialty

```csharp
public class GuideSpecialty : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }
}
```

#### District (İlçe / Semt)

```csharp
public class District : BaseEntity
{
    public string Name { get; set; }          // "Sultanahmet"
    public string Slug { get; set; }          // "sultanahmet"
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int SortOrder { get; set; }
    public bool IsPopular { get; set; }

    public ICollection<GuideDistrict> GuideDistricts { get; set; }
}
```

#### GuideDistrict

```csharp
public class GuideDistrict : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public Guid DistrictId { get; set; }
    public District District { get; set; }
}
```

#### GuideCertificate

```csharp
public class GuideCertificate : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public string CertificateName { get; set; }
    public string? IssuingAuthority { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? DocumentUrl { get; set; }
    public bool IsVerified { get; set; }
}
```

#### GuidePhoto

```csharp
public class GuidePhoto : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public string Url { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}
```

#### GuideAvailability

```csharp
public class GuideAvailability : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### Review (Değerlendirme)

```csharp
public class Review : BaseAuditableEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public string ReviewerName { get; set; }
    public string? ReviewerEmail { get; set; }
    public int Rating { get; set; }                   // 1-5
    public string? Comment { get; set; }
    public string? AdminReply { get; set; }
    public ReviewStatus Status { get; set; }          // Pending, Approved, Rejected
    public string? ReviewerCountry { get; set; }
    public string? ReviewerLanguage { get; set; }
}
```

#### ContactRequest (İletişim Talebi — İleride Booking olur)

```csharp
public class ContactRequest : BaseAuditableEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; }
    public string VisitorName { get; set; }
    public string VisitorEmail { get; set; }
    public string? VisitorPhone { get; set; }
    public string Message { get; set; }
    public DateTime? PreferredDate { get; set; }
    public int? GroupSize { get; set; }
    public ContactRequestStatus Status { get; set; }  // New, Viewed, Replied, Converted
    public string? AdminNotes { get; set; }
    public ContactSource Source { get; set; }          // Website, WhatsApp, Direct
}
```

#### PageContent (Statik Sayfa İçerikleri — CMS)

```csharp
public class PageContent : BaseAuditableEntity
{
    public string Key { get; set; }          // "about-us", "terms", "privacy"
    public string Title { get; set; }
    public string Content { get; set; }       // HTML content
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsPublished { get; set; }
}
```

#### SiteSettings (Site Ayarları)

```csharp
public class SiteSettings : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public string GroupName { get; set; }     // "General", "Contact", "Social"
}
```

### 2.5 Enum'lar

```csharp
public enum GuideStatus       { Pending, Approved, Rejected, Suspended }
public enum Gender             { Male, Female, Other, PreferNotToSay }
public enum LanguageProficiency { Native, Fluent, Intermediate, Basic }
public enum ReviewStatus       { Pending, Approved, Rejected }
public enum ContactRequestStatus { New, Viewed, Replied, Converted, Closed }
public enum ContactSource      { Website, WhatsApp, Direct, Referral }
```

### 2.6 Value Objects

```csharp
public class PhoneNumber : ValueObject
{
    public string CountryCode { get; }    // "+90"
    public string Number { get; }          // "5321234567"

    public string ToWhatsAppUrl() => $"https://wa.me/{CountryCode.TrimStart('+')}{Number}";
    public string ToFormatted() => $"{CountryCode} {Number}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return Number;
    }
}

public class Location : ValueObject
{
    public double Latitude { get; }
    public double Longitude { get; }
    public string? Address { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }  // "TRY", "USD", "EUR"

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### 2.7 Domain Events

```csharp
public record GuideRegisteredEvent(Guid GuideId) : IDomainEvent;
public record GuideApprovedEvent(Guid GuideId) : IDomainEvent;
public record GuideRejectedEvent(Guid GuideId, string Reason) : IDomainEvent;
public record ReviewSubmittedEvent(Guid ReviewId, Guid GuideId) : IDomainEvent;
public record ContactRequestCreatedEvent(Guid RequestId, Guid GuideId) : IDomainEvent;
```

### 2.8 Repository Interface'leri

```csharp
// Domain/Common/IRepository.cs
public interface IRepository<T> where T : BaseEntity, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}

// Domain/Repositories/IGuideRepository.cs
public interface IGuideRepository : IRepository<Guide>
{
    Task<Guide?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Guide?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<Guide>> GetApprovedGuidesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Guide>> GetFeaturedGuidesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Guide>> SearchGuidesAsync(GuideSearchCriteria criteria, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);
}

// Domain/Repositories/IReviewRepository.cs
public interface IReviewRepository : IRepository<Review>
{
    Task<IReadOnlyList<Review>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default);
    Task<IReadOnlyList<Review>> GetPendingReviewsAsync(CancellationToken ct = default);
    Task<double> GetAverageRatingAsync(Guid guideId, CancellationToken ct = default);
}

// Domain/Repositories/IContactRequestRepository.cs
public interface IContactRequestRepository : IRepository<ContactRequest>
{
    Task<IReadOnlyList<ContactRequest>> GetByGuideIdAsync(Guid guideId, CancellationToken ct = default);
    Task<int> GetNewRequestCountAsync(CancellationToken ct = default);
}
```

---

## 3. APPLICATION KATMANI (IstGuide.Application)

### 3.1 Yapı

```
Application/
├── Common/
│   ├── Interfaces/
│   │   ├── IApplicationDbContext.cs
│   │   ├── ICurrentUserService.cs
│   │   ├── IFileStorageService.cs
│   │   ├── ISlugService.cs
│   │   ├── IEmailService.cs
│   │   └── IWhatsAppService.cs
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs        ← MediatR pipeline
│   │   ├── LoggingBehavior.cs
│   │   └── PerformanceBehavior.cs
│   ├── Mappings/
│   │   └── MappingProfile.cs            ← AutoMapper
│   ├── Models/
│   │   ├── PaginatedList.cs
│   │   ├── Result.cs                    ← Result pattern (Success/Failure)
│   │   └── PagedRequest.cs
│   └── Exceptions/
│       ├── NotFoundException.cs
│       ├── ValidationException.cs
│       └── ForbiddenAccessException.cs
│
├── Features/
│   ├── Guides/
│   │   ├── Commands/
│   │   │   ├── RegisterGuide/
│   │   │   │   ├── RegisterGuideCommand.cs
│   │   │   │   ├── RegisterGuideCommandHandler.cs
│   │   │   │   └── RegisterGuideCommandValidator.cs
│   │   │   ├── UpdateGuideProfile/
│   │   │   │   ├── UpdateGuideProfileCommand.cs
│   │   │   │   ├── UpdateGuideProfileCommandHandler.cs
│   │   │   │   └── UpdateGuideProfileCommandValidator.cs
│   │   │   ├── ApproveGuide/
│   │   │   │   ├── ApproveGuideCommand.cs
│   │   │   │   └── ApproveGuideCommandHandler.cs
│   │   │   ├── RejectGuide/
│   │   │   │   ├── RejectGuideCommand.cs
│   │   │   │   └── RejectGuideCommandHandler.cs
│   │   │   ├── ToggleFeaturedGuide/
│   │   │   │   └── ...
│   │   │   ├── UploadGuidePhoto/
│   │   │   │   └── ...
│   │   │   └── DeleteGuidePhoto/
│   │   │       └── ...
│   │   ├── Queries/
│   │   │   ├── GetGuideBySlug/
│   │   │   │   ├── GetGuideBySlugQuery.cs
│   │   │   │   ├── GetGuideBySlugQueryHandler.cs
│   │   │   │   └── GuideDetailDto.cs
│   │   │   ├── GetApprovedGuides/
│   │   │   │   ├── GetApprovedGuidesQuery.cs
│   │   │   │   ├── GetApprovedGuidesQueryHandler.cs
│   │   │   │   └── GuideListDto.cs
│   │   │   ├── GetFeaturedGuides/
│   │   │   │   └── ...
│   │   │   ├── SearchGuides/
│   │   │   │   ├── SearchGuidesQuery.cs
│   │   │   │   ├── SearchGuidesQueryHandler.cs
│   │   │   │   └── GuideSearchResultDto.cs
│   │   │   ├── GetAllGuidesAdmin/
│   │   │   │   └── ...
│   │   │   └── GetGuideStatistics/
│   │   │       └── ...
│   │   └── EventHandlers/
│   │       ├── GuideRegisteredEventHandler.cs
│   │       └── GuideApprovedEventHandler.cs
│   │
│   ├── Reviews/
│   │   ├── Commands/
│   │   │   ├── SubmitReview/
│   │   │   │   ├── SubmitReviewCommand.cs
│   │   │   │   ├── SubmitReviewCommandHandler.cs
│   │   │   │   └── SubmitReviewCommandValidator.cs
│   │   │   ├── ApproveReview/
│   │   │   │   └── ...
│   │   │   ├── RejectReview/
│   │   │   │   └── ...
│   │   │   └── ReplyToReview/
│   │   │       └── ...
│   │   └── Queries/
│   │       ├── GetGuideReviews/
│   │       │   └── ...
│   │       └── GetPendingReviews/
│   │           └── ...
│   │
│   ├── ContactRequests/
│   │   ├── Commands/
│   │   │   ├── CreateContactRequest/
│   │   │   │   ├── CreateContactRequestCommand.cs
│   │   │   │   ├── CreateContactRequestCommandHandler.cs
│   │   │   │   └── CreateContactRequestCommandValidator.cs
│   │   │   └── UpdateContactRequestStatus/
│   │   │       └── ...
│   │   └── Queries/
│   │       ├── GetContactRequests/
│   │       │   └── ...
│   │       └── GetContactRequestStats/
│   │           └── ...
│   │
│   ├── Districts/
│   │   ├── Commands/
│   │   │   ├── CreateDistrict/
│   │   │   └── UpdateDistrict/
│   │   └── Queries/
│   │       ├── GetAllDistricts/
│   │       └── GetPopularDistricts/
│   │
│   ├── Languages/
│   │   └── Queries/
│   │       └── GetAllLanguages/
│   │
│   ├── Specialties/
│   │   ├── Commands/
│   │   │   ├── CreateSpecialty/
│   │   │   └── UpdateSpecialty/
│   │   └── Queries/
│   │       └── GetAllSpecialties/
│   │
│   ├── Pages/
│   │   ├── Commands/
│   │   │   └── UpdatePageContent/
│   │   └── Queries/
│   │       └── GetPageContent/
│   │
│   └── Dashboard/
│       └── Queries/
│           └── GetDashboardStats/
│               ├── GetDashboardStatsQuery.cs
│               ├── GetDashboardStatsQueryHandler.cs
│               └── DashboardStatsDto.cs
```

### 3.2 Temel DTO'lar

```csharp
// GuideListDto — Kart görünümü için
public class GuideListDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Languages { get; set; }
    public List<string> Specialties { get; set; }
    public List<string> Districts { get; set; }
    public bool IsFeatured { get; set; }
    public string WhatsAppUrl { get; set; }
}

// GuideDetailDto — Profil sayfası için
public class GuideDetailDto : GuideListDto
{
    public string? DetailedDescription { get; set; }
    public string? LicenseNumber { get; set; }
    public int YearsOfExperience { get; set; }
    public Gender Gender { get; set; }
    public List<GuidePhotoDto> Photos { get; set; }
    public List<GuideCertificateDto> Certificates { get; set; }
    public List<GuideAvailabilityDto> Availabilities { get; set; }
    public List<ReviewDto> RecentReviews { get; set; }
}

// GuideSearchCriteria
public class GuideSearchCriteria
{
    public string? SearchTerm { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? SpecialtyId { get; set; }
    public Guid? LanguageId { get; set; }
    public double? MinRating { get; set; }
    public string? SortBy { get; set; }       // "rating", "experience", "reviews"
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

// DashboardStatsDto
public class DashboardStatsDto
{
    public int TotalGuides { get; set; }
    public int PendingApprovals { get; set; }
    public int ApprovedGuides { get; set; }
    public int TotalReviews { get; set; }
    public int PendingReviews { get; set; }
    public int TotalContactRequests { get; set; }
    public int NewContactRequests { get; set; }
    public List<GuideListDto> RecentRegistrations { get; set; }
}
```

### 3.3 Örnek Command & Handler

```csharp
// RegisterGuideCommand.cs
public record RegisterGuideCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string Title { get; init; }
    public string Bio { get; init; }
    public int YearsOfExperience { get; init; }
    public Gender Gender { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string? LicenseNumber { get; init; }
    public List<Guid> LanguageIds { get; init; }
    public List<Guid> SpecialtyIds { get; init; }
    public List<Guid> DistrictIds { get; init; }
}

// RegisterGuideCommandHandler.cs
public class RegisterGuideCommandHandler : IRequestHandler<RegisterGuideCommand, Result<Guid>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly ISlugService _slugService;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<Guid>> Handle(RegisterGuideCommand request, CancellationToken ct)
    {
        // 1. Email benzersizliğini kontrol et
        var existing = await _guideRepository.GetByEmailAsync(request.Email, ct);
        if (existing != null)
            return Result<Guid>.Failure("Bu email adresi zaten kayıtlı.");

        // 2. Slug oluştur
        var slug = await _slugService.GenerateUniqueSlugAsync(
            $"{request.FirstName}-{request.LastName}-istanbul");

        // 3. Entity oluştur
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
            YearsOfExperience = request.YearsOfExperience,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            LicenseNumber = request.LicenseNumber,
            Status = GuideStatus.Pending,
            // Languages, Specialties, Districts eklenir...
        };

        guide.AddDomainEvent(new GuideRegisteredEvent(guide.Id));

        await _guideRepository.AddAsync(guide, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(guide.Id);
    }
}

// RegisterGuideCommandValidator.cs
public class RegisterGuideCommandValidator : AbstractValidator<RegisterGuideCommand>
{
    public RegisterGuideCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^[0-9]{10,11}$");
        RuleFor(x => x.Bio).NotEmpty().MaximumLength(500);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LanguageIds).NotEmpty().WithMessage("En az bir dil seçmelisiniz.");
        RuleFor(x => x.SpecialtyIds).NotEmpty().WithMessage("En az bir uzmanlık alanı seçmelisiniz.");
        RuleFor(x => x.DistrictIds).NotEmpty().WithMessage("En az bir bölge seçmelisiniz.");
    }
}
```

### 3.4 Örnek Query

```csharp
// SearchGuidesQuery.cs
public record SearchGuidesQuery : IRequest<PaginatedList<GuideListDto>>
{
    public GuideSearchCriteria Criteria { get; init; }
}

// SearchGuidesQueryHandler.cs
public class SearchGuidesQueryHandler
    : IRequestHandler<SearchGuidesQuery, PaginatedList<GuideListDto>>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IMapper _mapper;

    public async Task<PaginatedList<GuideListDto>> Handle(
        SearchGuidesQuery request, CancellationToken ct)
    {
        var guides = await _guideRepository.SearchGuidesAsync(request.Criteria, ct);
        return _mapper.Map<PaginatedList<GuideListDto>>(guides);
    }
}
```

---

## 4. INFRASTRUCTURE KATMANI

### 4.1 Yapı

```
Infrastructure/
├── Services/
│   ├── FileStorageService.cs        ← Yerel dosya, ileride Azure Blob
│   ├── SlugService.cs
│   ├── CurrentUserService.cs
│   ├── EmailService.cs              ← Faz 2
│   └── WhatsAppService.cs           ← URL oluşturma (API değil, link)
├── Identity/
│   ├── ApplicationUser.cs           ← IdentityUser<string> extend
│   ├── IdentityService.cs
│   └── JwtTokenService.cs           ← Faz 2
└── DependencyInjection.cs
```

### 4.2 Persistence Katmanı

```
Persistence/
├── ApplicationDbContext.cs
├── Configurations/
│   ├── GuideConfiguration.cs
│   ├── LanguageConfiguration.cs
│   ├── SpecialtyConfiguration.cs
│   ├── DistrictConfiguration.cs
│   ├── ReviewConfiguration.cs
│   ├── ContactRequestConfiguration.cs
│   └── ... (her entity için)
├── Repositories/
│   ├── GuideRepository.cs
│   ├── ReviewRepository.cs
│   └── ContactRequestRepository.cs
├── Interceptors/
│   ├── AuditableEntityInterceptor.cs
│   └── SoftDeleteInterceptor.cs
├── Migrations/
├── Seeds/
│   ├── LanguageSeed.cs
│   ├── SpecialtySeed.cs
│   └── DistrictSeed.cs
├── UnitOfWork.cs
└── DependencyInjection.cs
```

---

## 5. API KATMANI

### 5.1 Controller'lar

```
API/
├── Controllers/
│   ├── v1/
│   │   ├── GuidesController.cs
│   │   ├── ReviewsController.cs
│   │   ├── ContactRequestsController.cs
│   │   ├── DistrictsController.cs
│   │   ├── LanguagesController.cs
│   │   ├── SpecialtiesController.cs
│   │   ├── PagesController.cs
│   │   └── AuthController.cs
│   └── Admin/
│       ├── AdminGuidesController.cs
│       ├── AdminReviewsController.cs
│       ├── AdminDashboardController.cs
│       ├── AdminDistrictsController.cs
│       └── AdminSettingsController.cs
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
├── Filters/
│   └── ApiExceptionFilterAttribute.cs
└── Program.cs
```

### 5.2 Endpoint Listesi

```
── PUBLIC API (/api/v1) ───────────────────────────────────
GET    /guides                          → SearchGuides (filtreleme + sayfalama)
GET    /guides/featured                 → GetFeaturedGuides
GET    /guides/{slug}                   → GetGuideBySlug
POST   /guides/register                → RegisterGuide
PUT    /guides/{id}/profile             → UpdateGuideProfile
POST   /guides/{id}/photos              → UploadGuidePhoto
DELETE /guides/{id}/photos/{photoId}    → DeleteGuidePhoto

GET    /guides/{guideId}/reviews        → GetGuideReviews
POST   /guides/{guideId}/reviews        → SubmitReview

POST   /contact-requests                → CreateContactRequest

GET    /districts                       → GetAllDistricts
GET    /districts/popular               → GetPopularDistricts
GET    /languages                       → GetAllLanguages
GET    /specialties                     → GetAllSpecialties
GET    /pages/{key}                     → GetPageContent

POST   /auth/login                      → Login (admin)
POST   /auth/register                   → Register (rehber)

── ADMIN API (/api/admin) ─────────────────────────────────
GET    /dashboard/stats                 → GetDashboardStats

GET    /guides                          → GetAllGuidesAdmin (tüm statülerle)
GET    /guides/{id}                     → GetGuideDetailAdmin
PUT    /guides/{id}/approve             → ApproveGuide
PUT    /guides/{id}/reject              → RejectGuide
PUT    /guides/{id}/suspend             → SuspendGuide
PUT    /guides/{id}/toggle-featured     → ToggleFeaturedGuide

GET    /reviews                         → GetAllReviews
GET    /reviews/pending                 → GetPendingReviews
PUT    /reviews/{id}/approve            → ApproveReview
PUT    /reviews/{id}/reject             → RejectReview
PUT    /reviews/{id}/reply              → ReplyToReview

GET    /contact-requests                → GetContactRequests
PUT    /contact-requests/{id}/status    → UpdateContactRequestStatus

POST   /districts                       → CreateDistrict
PUT    /districts/{id}                  → UpdateDistrict
POST   /specialties                     → CreateSpecialty
PUT    /specialties/{id}                → UpdateSpecialty
PUT    /pages/{key}                     → UpdatePageContent
```

---

## 6. MSSQL EXPRESS VERİTABANI ŞEMASI

### 6.1 Tablolar

```sql
-- ============================================
-- IstGuide.me Database Schema — MSSQL Express
-- ============================================

-- ASP.NET Identity tabloları EF tarafından oluşturulur
-- (AspNetUsers, AspNetRoles, AspNetUserRoles, vb.)

-- ──────────── Ana Tablolar ────────────

CREATE TABLE Guides (
    Id                  UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    FirstName           NVARCHAR(100)    NOT NULL,
    LastName            NVARCHAR(100)    NOT NULL,
    Email               NVARCHAR(256)    NOT NULL,
    PhoneNumber         NVARCHAR(20)     NOT NULL,
    WhatsAppUrl         NVARCHAR(200)    NULL,
    ProfilePhotoUrl     NVARCHAR(500)    NULL,
    CoverPhotoUrl       NVARCHAR(500)    NULL,
    DateOfBirth         DATE             NOT NULL,
    Gender              INT              NOT NULL DEFAULT 0,
    Slug                NVARCHAR(200)    NOT NULL,
    Title               NVARCHAR(200)    NOT NULL,
    Bio                 NVARCHAR(500)    NOT NULL,
    DetailedDescription NVARCHAR(MAX)    NULL,
    LicenseNumber       NVARCHAR(50)     NULL,
    YearsOfExperience   INT              NOT NULL DEFAULT 0,
    HourlyRate          DECIMAL(10,2)    NULL,
    DailyRate           DECIMAL(10,2)    NULL,
    Status              INT              NOT NULL DEFAULT 0,  -- Pending
    IsFeatured          BIT              NOT NULL DEFAULT 0,
    ProfileViewCount    INT              NOT NULL DEFAULT 0,
    AverageRating       FLOAT            NOT NULL DEFAULT 0,
    ReviewCount         INT              NOT NULL DEFAULT 0,
    UserId              NVARCHAR(450)    NULL,
    CreatedAt           DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt           DATETIME2        NULL,
    CreatedBy           NVARCHAR(256)    NULL,
    UpdatedBy           NVARCHAR(256)    NULL,
    IsDeleted           BIT              NOT NULL DEFAULT 0,

    CONSTRAINT UQ_Guides_Email UNIQUE (Email),
    CONSTRAINT UQ_Guides_Slug  UNIQUE (Slug),
    CONSTRAINT FK_Guides_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Guides_Status     ON Guides(Status) WHERE IsDeleted = 0;
CREATE INDEX IX_Guides_IsFeatured ON Guides(IsFeatured) WHERE IsDeleted = 0 AND Status = 1;
CREATE INDEX IX_Guides_Slug       ON Guides(Slug) WHERE IsDeleted = 0;

-- ──────────── Lookup Tabloları ────────────

CREATE TABLE Languages (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Name        NVARCHAR(100)    NOT NULL,
    Code        NVARCHAR(10)     NOT NULL,
    NativeName  NVARCHAR(100)    NOT NULL,
    FlagIconUrl NVARCHAR(500)    NULL,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0
);

CREATE TABLE Specialties (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Name        NVARCHAR(200)    NOT NULL,
    Slug        NVARCHAR(200)    NOT NULL,
    Description NVARCHAR(500)    NULL,
    IconUrl     NVARCHAR(500)    NULL,
    SortOrder   INT              NOT NULL DEFAULT 0,
    IsActive    BIT              NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0
);

CREATE TABLE Districts (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Name        NVARCHAR(200)    NOT NULL,
    Slug        NVARCHAR(200)    NOT NULL,
    Description NVARCHAR(500)    NULL,
    ImageUrl    NVARCHAR(500)    NULL,
    Latitude    FLOAT            NULL,
    Longitude   FLOAT            NULL,
    SortOrder   INT              NOT NULL DEFAULT 0,
    IsPopular   BIT              NOT NULL DEFAULT 0,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0
);

-- ──────────── Junction Tabloları ────────────

CREATE TABLE GuideLanguages (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId     UNIQUEIDENTIFIER NOT NULL,
    LanguageId  UNIQUEIDENTIFIER NOT NULL,
    Proficiency INT              NOT NULL DEFAULT 0,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GL_Guide    FOREIGN KEY (GuideId)    REFERENCES Guides(Id),
    CONSTRAINT FK_GL_Language FOREIGN KEY (LanguageId) REFERENCES Languages(Id),
    CONSTRAINT UQ_GL          UNIQUE (GuideId, LanguageId)
);

CREATE TABLE GuideSpecialties (
    Id           UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId      UNIQUEIDENTIFIER NOT NULL,
    SpecialtyId  UNIQUEIDENTIFIER NOT NULL,
    CreatedAt    DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt    DATETIME2        NULL,
    IsDeleted    BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GS_Guide      FOREIGN KEY (GuideId)     REFERENCES Guides(Id),
    CONSTRAINT FK_GS_Specialty   FOREIGN KEY (SpecialtyId) REFERENCES Specialties(Id),
    CONSTRAINT UQ_GS             UNIQUE (GuideId, SpecialtyId)
);

CREATE TABLE GuideDistricts (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId     UNIQUEIDENTIFIER NOT NULL,
    DistrictId  UNIQUEIDENTIFIER NOT NULL,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GD_Guide    FOREIGN KEY (GuideId)    REFERENCES Guides(Id),
    CONSTRAINT FK_GD_District FOREIGN KEY (DistrictId) REFERENCES Districts(Id),
    CONSTRAINT UQ_GD          UNIQUE (GuideId, DistrictId)
);

-- ──────────── Rehber Alt Tabloları ────────────

CREATE TABLE GuideCertificates (
    Id               UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId          UNIQUEIDENTIFIER NOT NULL,
    CertificateName  NVARCHAR(200)    NOT NULL,
    IssuingAuthority NVARCHAR(200)    NULL,
    IssueDate        DATE             NULL,
    ExpiryDate       DATE             NULL,
    DocumentUrl      NVARCHAR(500)    NULL,
    IsVerified       BIT              NOT NULL DEFAULT 0,
    CreatedAt        DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt        DATETIME2        NULL,
    IsDeleted        BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GC_Guide FOREIGN KEY (GuideId) REFERENCES Guides(Id)
);

CREATE TABLE GuidePhotos (
    Id           UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId      UNIQUEIDENTIFIER NOT NULL,
    Url          NVARCHAR(500)    NOT NULL,
    ThumbnailUrl NVARCHAR(500)    NULL,
    Caption      NVARCHAR(200)    NULL,
    SortOrder    INT              NOT NULL DEFAULT 0,
    IsPrimary    BIT              NOT NULL DEFAULT 0,
    CreatedAt    DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt    DATETIME2        NULL,
    IsDeleted    BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GP_Guide FOREIGN KEY (GuideId) REFERENCES Guides(Id)
);

CREATE TABLE GuideAvailabilities (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId     UNIQUEIDENTIFIER NOT NULL,
    DayOfWeek   INT              NOT NULL,    -- 0=Sunday ... 6=Saturday
    StartTime   TIME             NOT NULL,
    EndTime     TIME             NOT NULL,
    IsAvailable BIT              NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_GA_Guide FOREIGN KEY (GuideId) REFERENCES Guides(Id)
);

-- ──────────── Değerlendirmeler ────────────

CREATE TABLE Reviews (
    Id               UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId          UNIQUEIDENTIFIER NOT NULL,
    ReviewerName     NVARCHAR(200)    NOT NULL,
    ReviewerEmail    NVARCHAR(256)    NULL,
    Rating           INT              NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment          NVARCHAR(1000)   NULL,
    AdminReply       NVARCHAR(1000)   NULL,
    Status           INT              NOT NULL DEFAULT 0,  -- Pending
    ReviewerCountry  NVARCHAR(100)    NULL,
    ReviewerLanguage NVARCHAR(50)     NULL,
    CreatedAt        DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt        DATETIME2        NULL,
    CreatedBy        NVARCHAR(256)    NULL,
    UpdatedBy        NVARCHAR(256)    NULL,
    IsDeleted        BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_R_Guide FOREIGN KEY (GuideId) REFERENCES Guides(Id)
);

CREATE INDEX IX_Reviews_GuideId ON Reviews(GuideId) WHERE IsDeleted = 0;
CREATE INDEX IX_Reviews_Status  ON Reviews(Status)  WHERE IsDeleted = 0;

-- ──────────── İletişim Talepleri ────────────

CREATE TABLE ContactRequests (
    Id            UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    GuideId       UNIQUEIDENTIFIER NOT NULL,
    VisitorName   NVARCHAR(200)    NOT NULL,
    VisitorEmail  NVARCHAR(256)    NOT NULL,
    VisitorPhone  NVARCHAR(20)     NULL,
    Message       NVARCHAR(2000)   NOT NULL,
    PreferredDate DATE             NULL,
    GroupSize     INT              NULL,
    Status        INT              NOT NULL DEFAULT 0,  -- New
    AdminNotes    NVARCHAR(1000)   NULL,
    Source        INT              NOT NULL DEFAULT 0,  -- Website
    CreatedAt     DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt     DATETIME2        NULL,
    CreatedBy     NVARCHAR(256)    NULL,
    UpdatedBy     NVARCHAR(256)    NULL,
    IsDeleted     BIT              NOT NULL DEFAULT 0,

    CONSTRAINT FK_CR_Guide FOREIGN KEY (GuideId) REFERENCES Guides(Id)
);

-- ──────────── CMS & Ayarlar ────────────

CREATE TABLE PageContents (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Key]           NVARCHAR(100)    NOT NULL,
    Title           NVARCHAR(200)    NOT NULL,
    Content         NVARCHAR(MAX)    NOT NULL,
    MetaTitle       NVARCHAR(200)    NULL,
    MetaDescription NVARCHAR(500)    NULL,
    IsPublished     BIT              NOT NULL DEFAULT 0,
    CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2        NULL,
    CreatedBy       NVARCHAR(256)    NULL,
    UpdatedBy       NVARCHAR(256)    NULL,
    IsDeleted       BIT              NOT NULL DEFAULT 0,

    CONSTRAINT UQ_PageContents_Key UNIQUE ([Key])
);

CREATE TABLE SiteSettings (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Key]       NVARCHAR(100)    NOT NULL,
    Value       NVARCHAR(MAX)    NOT NULL,
    Description NVARCHAR(500)    NULL,
    GroupName   NVARCHAR(100)    NOT NULL DEFAULT 'General',
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,
    IsDeleted   BIT              NOT NULL DEFAULT 0,

    CONSTRAINT UQ_SiteSettings_Key UNIQUE ([Key])
);
```

### 6.2 Seed Data

```sql
-- Diller
INSERT INTO Languages (Id, Name, Code, NativeName) VALUES
(NEWID(), 'İngilizce',  'en', 'English'),
(NEWID(), 'Almanca',    'de', 'Deutsch'),
(NEWID(), 'Fransızca',  'fr', 'Français'),
(NEWID(), 'İspanyolca', 'es', 'Español'),
(NEWID(), 'Arapça',     'ar', 'العربية'),
(NEWID(), 'Rusça',      'ru', 'Русский'),
(NEWID(), 'Japonca',    'ja', '日本語'),
(NEWID(), 'Korece',     'ko', '한국어'),
(NEWID(), 'Çince',      'zh', '中文'),
(NEWID(), 'Portekizce', 'pt', 'Português'),
(NEWID(), 'İtalyanca',  'it', 'Italiano'),
(NEWID(), 'Türkçe',     'tr', 'Türkçe');

-- Uzmanlık Alanları
INSERT INTO Specialties (Id, Name, Slug, SortOrder) VALUES
(NEWID(), 'Tarihi Turlar',              'tarihi-turlar',          1),
(NEWID(), 'Gastronomi Turları',         'gastronomi-turlari',     2),
(NEWID(), 'Boğaz Turları',              'bogaz-turlari',          3),
(NEWID(), 'Fotoğraf Turları',           'fotograf-turlari',       4),
(NEWID(), 'Müze & Sanat',               'muze-sanat',             5),
(NEWID(), 'Mimari Turlar',              'mimari-turlar',          6),
(NEWID(), 'Alışveriş Turları',          'alisveris-turlari',      7),
(NEWID(), 'Gece Turları',               'gece-turlari',           8),
(NEWID(), 'Doğa & Trekking',            'doga-trekking',          9),
(NEWID(), 'Özel VIP Turlar',            'ozel-vip-turlar',       10);

-- İstanbul İlçe / Bölgeleri
INSERT INTO Districts (Id, Name, Slug, IsPopular, SortOrder) VALUES
(NEWID(), 'Sultanahmet',      'sultanahmet',       1, 1),
(NEWID(), 'Eminönü',          'eminonu',            1, 2),
(NEWID(), 'Beyoğlu / Taksim', 'beyoglu-taksim',     1, 3),
(NEWID(), 'Karaköy / Galata', 'karakoy-galata',     1, 4),
(NEWID(), 'Beşiktaş',         'besiktas',           1, 5),
(NEWID(), 'Kadıköy',          'kadikoy',            1, 6),
(NEWID(), 'Üsküdar',          'uskudar',            1, 7),
(NEWID(), 'Balat / Fener',    'balat-fener',        1, 8),
(NEWID(), 'Ortaköy',          'ortakoy',            0, 9),
(NEWID(), 'Sarıyer',          'sariyer',            0, 10),
(NEWID(), 'Adalar',           'adalar',             1, 11),
(NEWID(), 'Eyüpsultan',       'eyupsultan',         0, 12),
(NEWID(), 'Fatih',            'fatih',              0, 13),
(NEWID(), 'Bakırköy',         'bakirkoy',           0, 14),
(NEWID(), 'Bebek',            'bebek',              0, 15);

-- Site Ayarları
INSERT INTO SiteSettings ([Key], Value, GroupName) VALUES
('site_name',       'IstGuide',                    'General'),
('site_tagline',    'İstanbul Rehber Platformu',   'General'),
('contact_email',   'info@istguide.me',            'Contact'),
('contact_phone',   '+902121234567',               'Contact'),
('whatsapp_number', '+905321234567',               'Contact'),
('instagram_url',   'https://instagram.com/istguide', 'Social'),
('facebook_url',    '',                            'Social'),
('twitter_url',     '',                            'Social');
```

---

## 7. NuGet PAKETLER

```xml
<!-- Domain → Sıfır bağımlılık -->

<!-- Application -->
MediatR
MediatR.Extensions.Microsoft.DependencyInjection
FluentValidation
FluentValidation.DependencyInjectionExtensions
AutoMapper
AutoMapper.Extensions.Microsoft.DependencyInjection

<!-- Persistence -->
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Identity.EntityFrameworkCore

<!-- Infrastructure -->
Microsoft.Extensions.Configuration
Microsoft.Extensions.DependencyInjection

<!-- API -->
Swashbuckle.AspNetCore
Microsoft.AspNetCore.Authentication.JwtBearer
Serilog.AspNetCore
```

---

## 8. FRONTEND SAYFA HARİTASI

### 8.1 Public Site (istguide.me)

```
/                              → Ana Sayfa (hero + arama + öne çıkan rehberler + bölgeler)
/rehberler                     → Rehber Listesi (filtreler: dil, bölge, uzmanlık, puan)
/rehber/{slug}                 → Rehber Profil Detay (fotoğraflar, bilgiler, yorumlar, WhatsApp)
/rehber-ol                     → Rehber Kayıt Formu (çok adımlı)
/hakkimizda                    → Hakkımızda
/iletisim                      → İletişim
/gizlilik-politikasi           → Gizlilik Politikası
/kullanim-sartlari             → Kullanım Şartları
/bolgeler/{slug}               → Bölge Detay (o bölgedeki rehberler)
```

### 8.2 Admin Panel (/admin)

```
/admin                         → Dashboard (istatistikler, bekleyen onaylar)
/admin/rehberler               → Rehber Listesi (tüm statüler, arama, filtre)
/admin/rehberler/{id}          → Rehber Detay (onay/red/askıya al)
/admin/yorumlar                → Yorum Yönetimi (bekleyenler, onaylananlar)
/admin/iletisim-talepleri      → İletişim Talepleri
/admin/bolgeler                → Bölge Yönetimi (CRUD)
/admin/uzmanliklar             → Uzmanlık Alanı Yönetimi (CRUD)
/admin/diller                  → Dil Yönetimi (CRUD)
/admin/sayfalar                → Statik Sayfa Yönetimi (CMS)
/admin/ayarlar                 → Site Ayarları
```

---

## 9. TEMA & TASARIM REHBERİ

GetYourGuide'dan esinlenen minimalist tasarım:

```css
:root {
    /* Ana Renkler */
    --color-primary:      #FF5533;     /* Turuncu-kırmızı (CTA butonları) */
    --color-primary-dark:  #E04328;
    --color-primary-light: #FF7755;

    /* Nötr Renkler */
    --color-bg:            #FFFFFF;     /* Arkaplan beyaz */
    --color-surface:       #F7F7F7;    /* Kart arkaplanları */
    --color-border:        #E5E5E5;
    --color-text-primary:  #1A1A1A;
    --color-text-secondary:#6B6B6B;
    --color-text-muted:    #9B9B9B;

    /* Durum Renkleri */
    --color-success:       #00A67E;
    --color-warning:       #FFB800;
    --color-error:         #E53E3E;
    --color-info:          #3182CE;

    /* Rating Yıldız */
    --color-star:          #FFB800;

    /* WhatsApp */
    --color-whatsapp:      #25D366;

    /* Tipografi */
    --font-family:         'Inter', -apple-system, sans-serif;
    --font-size-xs:        0.75rem;
    --font-size-sm:        0.875rem;
    --font-size-base:      1rem;
    --font-size-lg:        1.125rem;
    --font-size-xl:        1.25rem;
    --font-size-2xl:       1.5rem;
    --font-size-3xl:       2rem;

    /* Spacing */
    --spacing-xs:          0.25rem;
    --spacing-sm:          0.5rem;
    --spacing-md:          1rem;
    --spacing-lg:          1.5rem;
    --spacing-xl:          2rem;
    --spacing-2xl:         3rem;

    /* Border Radius */
    --radius-sm:           0.375rem;
    --radius-md:           0.5rem;
    --radius-lg:           0.75rem;
    --radius-xl:           1rem;
    --radius-full:         50%;

    /* Shadows */
    --shadow-sm:           0 1px 2px rgba(0,0,0,0.05);
    --shadow-md:           0 4px 6px rgba(0,0,0,0.07);
    --shadow-lg:           0 10px 15px rgba(0,0,0,0.1);
}
```

### Tasarım Prensipleri

1. **Beyaz arkaplan**, içerik kartlarda `#F7F7F7` yüzeylerde
2. **Temiz tipografi**, Inter font ailesi
3. **Bol boşluk** (white space), bilgi kalabalığı yok
4. **Rehber kartları**: Profil fotoğraf + isim + puan + diller + kısa bio + WhatsApp butonu
5. **Hero alanı**: İstanbul görseli + arama kutusu (GetYourGuide tarzı)
6. **CTA butonları**: Turuncu-kırmızı (#FF5533)
7. **WhatsApp butonu**: Yeşil, her rehber kartında ve profil sayfasında belirgin
8. **Mobile-first** responsive tasarım

---

## 10. ÖLÇEKLENDİRME PLANI (Fazlar)

### Faz 1 (MVP — Şu an)
- Rehber kayıt ve profil
- Admin onay sistemi
- Arama ve filtreleme
- WhatsApp ile iletişim
- Yorum sistemi
- Statik sayfalar (CMS)

### Faz 2 (Büyüme)
- Online ödeme entegrasyonu (iyzico / Stripe)
- Tur paketi oluşturma (TourPackage entity)
- Booking sistemi (Booking entity, takvim)
- Rehber paneli (kendi profilini yönetme)
- Email bildirimler
- JWT authentication

### Faz 3 (Ölçek)
- Çoklu şehir desteği (City entity, şehir bazlı yapı)
- Çoklu dil desteği (i18n)
- Blog / içerik yönetimi
- Harita entegrasyonu (Google Maps)
- Analytics dashboard
- API rate limiting, caching (Redis)
- Mesajlaşma sistemi (in-app messaging)

### Faz 4 (Platform)
- Mobil uygulama (React Native / MAUI)
- Rehber doğrulama sistemi (TUREB API)
- Komisyon sistemi
- Affiliate / referral
- A/B testing
- Mikroservis dönüşümü (gerektiğinde)

---

## 11. CONNECTION STRING

```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=IstGuideDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 12. ÖZET MİMARİ DİYAGRAMI

```
┌─────────────────────────────────────────────────────┐
│                    CLIENTS                          │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │ Public   │  │ Admin Panel  │  │  Mobile App  │  │
│  │ Website  │  │ (Blazor/SPA) │  │  (Faz 4)     │  │
│  └────┬─────┘  └──────┬───────┘  └──────┬───────┘  │
└───────┼────────────────┼─────────────────┼──────────┘
        │                │                 │
        ▼                ▼                 ▼
┌─────────────────────────────────────────────────────┐
│              API LAYER (ASP.NET Core)               │
│  Controllers → Middleware → Filters → Swagger       │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────┐
│           APPLICATION LAYER (CQRS + MediatR)        │
│  Commands → Queries → Validators → Behaviors → DTOs│
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────┐
│              DOMAIN LAYER (Pure C#)                 │
│  Entities → Value Objects → Events → Interfaces     │
│  BaseEntity → BaseAuditableEntity → IAggregateRoot  │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────┐
│     INFRASTRUCTURE + PERSISTENCE LAYER              │
│  EF Core → Repositories → DbContext → Migrations   │
│  FileStorage → Identity → Interceptors → Seeds      │
└────────────────────────┬────────────────────────────┘
                         │
                         ▼
                 ┌────────────────┐
                 │  MSSQL Express │
                 │  IstGuideDb    │
                 └────────────────┘
```

---

## 13. PROJEDEKİ GÜNCEL DEĞİŞİKLİKLER (2026-04-05)

### 13.1 Yeni Eklenen Entity: Tour

**Dosya:** `src/IstGuide.Domain/Entities/Tour.cs`

```csharp
public class Tour : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Duration { get; set; } = string.Empty;  // "4 Hours", "Full Day"
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
}
```

- `BaseEntity`'den türetilir (CreatedBy/UpdatedBy yok)
- Navigation property yok (Guide veya District ile ilişki yok)
- EF Core configuration dosyası yok
- `ApplicationDbContext`'te `DbSet<Tour> Tours` olarak tanımlı
- Seed data ile 1 örnek tur ekleniyor ("Old City & Hagia Sophia", 150₺, 4 saat)

### 13.2 Güncellenen NuGet Paketleri (Mevcut Versiyonlar)

```xml
<!-- IstGuide.Domain → Sıfır bağımlılık (değişmedi) -->

<!-- IstGuide.Application -->
AutoMapper                                          v12.0.1
AutoMapper.Extensions.Microsoft.DependencyInjection  v12.0.1
FluentValidation                                    v12.1.1
FluentValidation.DependencyInjectionExtensions       v12.1.1
MediatR                                             v14.1.0
Microsoft.EntityFrameworkCore                        v9.0.5

<!-- IstGuide.Infrastructure -->
Microsoft.AspNetCore.Identity.EntityFrameworkCore     v9.0.5
System.IdentityModel.Tokens.Jwt                      v8.6.0

<!-- IstGuide.Persistence -->
Microsoft.AspNetCore.Identity.EntityFrameworkCore     v9.0.5
Microsoft.EntityFrameworkCore.SqlServer               v9.0.5
Microsoft.EntityFrameworkCore.Tools                   v9.0.5

<!-- IstGuide.API -->
Microsoft.AspNetCore.Authentication.JwtBearer         v9.0.5
Microsoft.AspNetCore.OpenApi                          v9.0.5
Microsoft.EntityFrameworkCore.Design                  v9.0.5
Serilog.AspNetCore                                    v10.0.0
```

**Planlanan ama henüz eklenmeyen paketler:** Swashbuckle.AspNetCore (Swagger kapalı)

### 13.3 Güncellenen Connection String

**SQL Server 2022 (Varsayılan Instance) kullanılıyor — SQL Express değil**

```json
// appsettings.json ve appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IstGuideDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False"
  }
}
```

**Değişiklikler:**
- `Server=.\\SQLEXPRESS` → `Server=localhost` (SQL Server 2022 varsayılan instance)
- `Encrypt=False` eklendi
- JWT ayarları sadece `appsettings.Development.json` içinde

### 13.4 Mevcut Middleware Dosyaları

**Planlanan:** ExceptionHandlingMiddleware + RequestLoggingMiddleware
**Mevcut:** Sadece ExceptionHandlingMiddleware

```
API/Middlewares/
└── ExceptionHandlingMiddleware.cs    ← Tüm unhandled exception'ları yakalar, JSON 500 döner
```

RequestLoggingMiddleware yok — Serilog.AspNetCore'un built-in request logging'i kullanılıyor.

ApiExceptionFilterAttribute yok.

### 13.5 Mevcut Interceptor'lar (Planlandığı gibi mevcut)

```
Persistence/Interceptors/
├── AuditableEntityInterceptor.cs     ← CreatedAt/CreatedBy, UpdatedAt/UpdatedBy otomatik
└── SoftDeleteInterceptor.cs          ← Delete yerine IsDeleted = true
```

Her iki interceptor da `ApplicationDbContext`'te `optionsBuilder.AddInterceptors()` ile kayıtlı.

### 13.6 Mevcut Seed Data

| Tablo | Kayıt Sayısı | Detay |
|-------|-------------|-------|
| Languages | 12 | İngilizce, Almanca, Fransızca, İspanyolca, Arapça, Rusça, Japonca, Korece, Çince, Portekizce, İtalyanca, Türkçe |
| Specialties | 10 | Tarihi Turlar, Gastronomi, Boğaz, Fotoğraf, Müze & Sanat, Mimari, Alışveriş, Gece, Doğa & Trekking, VIP |
| Districts | 15 | 8 popüler + 7 normal ilçe |
| SiteSettings | 8 | General, Contact, Social grupları |
| Tours | 1 | "Old City & Hagia Sophia" (150₺, 4 Hours, featured) |
| Guides | 1 | "Zafer Admin" (admin@istguide.me, Approved, 10 yıl deneyim) |

### 13.7 Mevcut Migration'lar

```
Persistence/Migrations/
├── 20260405084418_InitialCreate.cs
├── 20260405115304_FixModelChanges.cs          ← Tour tablosu bu migration'da eklendi
└── 20260405121816_AddGuideAvailabilityConfig.cs ← GuideAvailability Id default eklendi
```

### 13.8 Güncellenen API Endpoint Listesi

#### PUBLIC API (`/api/v1`) — ✅ Mevcut ve Çalışan

```
GET    /api/v1/guides                          → Rehber arama (filtreleme + sayfalama) ✅
GET    /api/v1/guides/featured                 → Öne çıkan rehberler ✅
GET    /api/v1/guides/{slug}                   → Rehber detay ✅
POST   /api/v1/guides/register                 → Rehber kayıt [AllowAnonymous] ✅
PUT    /api/v1/guides/{id}/profile             → Profil güncelleme [Authorize] ✅
POST   /api/v1/guides/{id}/photos              → Fotoğraf yükleme [Authorize] ✅
DELETE /api/v1/guides/{id}/photos/{photoId}    → Fotoğraf silme [Authorize] ✅

GET    /api/v1/guides/{guideId}/reviews        → Rehber yorumları ✅
POST   /api/v1/guides/{guideId}/reviews        → Yorum gönder (ReviewerEmail optional) ✅

POST   /api/v1/contact-requests                → İletişim talebi oluştur ✅

GET    /api/v1/districts                       → Tüm ilçeler ✅
GET    /api/v1/districts/popular               → Popüler ilçeler ✅
GET    /api/v1/languages                       → Tüm diller ✅
GET    /api/v1/specialties                     → Tüm uzmanlık alanları ✅
GET    /api/v1/pages/{key}                     → Statik sayfa içeriği ✅

POST   /api/v1/auth/login                      → Login (JWT) [AllowAnonymous] ✅
POST   /api/v1/auth/register                   → Kullanıcı kayıt [AllowAnonymous] ✅

GET    /api/v1/tours                           → Tüm turlar [AllowAnonymous] ✅
POST   /api/v1/tours                           → Tur oluştur ✅
```

#### ADMIN API (`/api/admin`) — ✅ Mevcut

```
GET    /api/admin/dashboard/stats              → Dashboard istatistikleri [Authorize(Roles="Admin")] ✅

GET    /api/admin/guides                       → Tüm rehberler (status filter) [Authorize(Roles="Admin")] ✅
GET    /api/admin/guides/{id}                  → Rehber detay [Authorize(Roles="Admin")] ✅
PUT    /api/admin/guides/{id}/approve          → Rehber onayla [Authorize(Roles="Admin")] ✅
PUT    /api/admin/guides/{id}/reject           → Rehber reddet [Authorize(Roles="Admin")] ✅
PUT    /api/admin/guides/{id}/suspend          → Rehber askıya al [Authorize(Roles="Admin")] ✅
PUT    /api/admin/guides/{id}/toggle-featured  → Öne çıkar [Authorize(Roles="Admin")] ✅

GET    /api/admin/reviews                      → Tüm yorumlar [Authorize(Roles="Admin")] ✅
GET    /api/admin/reviews/pending              → Bekleyen yorumlar [Authorize(Roles="Admin")] ✅
PUT    /api/admin/reviews/{id}/approve         → Yorum onayla [Authorize(Roles="Admin")] ✅
PUT    /api/admin/reviews/{id}/reject          → Yorum reddet [Authorize(Roles="Admin")] ✅
PUT    /api/admin/reviews/{id}/reply           → Yoruma cevap ver [Authorize(Roles="Admin")] ✅

GET    /api/admin/districts                    → Tüm ilçeler [Authorize(Roles="Admin")] ✅
GET    /api/admin/districts/popular            → Popüler ilçeler [Authorize(Roles="Admin")] ✅
POST   /api/admin/districts                    → İlçe oluştur [Authorize(Roles="Admin")] ✅
PUT    /api/admin/districts/{id}               → İlçe güncelle [Authorize(Roles="Admin")] ✅

GET    /api/admin/contact-requests             → İletişim talepleri [Authorize(Roles="Admin")] ✅
GET    /api/admin/contact-requests/stats       → İletişim istatistikleri [Authorize(Roles="Admin")] ✅

GET    /api/admin/specialties                  → Tüm uzmanlıklar [Authorize(Roles="Admin")] ✅
POST   /api/admin/specialties                  → Uzmanlık oluştur [Authorize(Roles="Admin")] ✅
PUT    /api/admin/specialties/{id}             → Uzmanlık güncelle [Authorize(Roles="Admin")] ✅
GET    /api/admin/pages/{key}                  → Sayfa içeriği [Authorize(Roles="Admin")] ✅
PUT    /api/admin/pages/{key}                  → Sayfa güncelle [Authorize(Roles="Admin")] ✅
```

### 13.9 Yapılan Kod Düzeltmeleri

#### SubmitReviewCommand — ReviewerEmail Optional Yapıldı

**Dosya:** `src/IstGuide.Application/Features/Reviews/Commands/SubmitReview/SubmitReviewCommand.cs`

```csharp
// Önceki (hata veriyordu)
public record SubmitReviewCommand(Guid GuideId, string ReviewerName, string ReviewerEmail, int Rating, string Comment)

// Güncel
public record SubmitReviewCommand(Guid GuideId, string ReviewerName, string? ReviewerEmail, int Rating, string? Comment)
```

#### Seed Data — Guide ve Tour Zorunlu Alanları Eklendi

**Dosya:** `src/IstGuide.Persistence/Seeds/ApplicationDbContextSeed.cs`

Guide seed'e eklenen alanlar:
- `Slug` = "zafer-admin-istanbul"
- `DateOfBirth` = 1990-01-01
- `Gender` = Gender.Male
- `YearsOfExperience` = 10

Tour seed'e eklenen alanlar:
- `Slug` = "old-city-hagia-sophia"
- `IsActive` = true
- `IsFeatured` = true

### 13.10 Mimarideki Farklar (Planlanan vs Mevcut)

| Öğe | Planlanan | Mevcut | Durum |
|-----|-----------|--------|-------|
| **Tour Entity** | Yok | ✅ Var | Yeni eklendi |
| **ToursController** | Yok | ✅ Var (`/api/v1/tours`) | Yeni eklendi |
| **AdminContactRequestsController** | Planlandı | ✅ Var | Uygulandı |
| **RequestLoggingMiddleware** | Planlandı | ❌ Yok | Serilog built-in kullanılıyor |
| **ApiExceptionFilterAttribute** | Planlandı | ❌ Yok | ExceptionHandlingMiddleware var |
| **PerformanceBehavior** | Planlandı | ❌ Yok | Sadece Logging + Validation |
| **PagedRequest.cs** | Planlandı | ❌ Yok | PaginatedList var |
| **Swashbuckle (Swagger)** | Planlandı | ❌ Kapalı | Henüz eklenmedi |
| **EmailService** | Faz 2 | ❌ Yok | Interface var, impl yok |
| **JwtTokenService** | Faz 2 | ❌ Yok | JwtBearer var ama token servisi yok |
| **WebUI (React/Vite)** | Planlandı | ✅ Var (boş) | Scaffolded ama implement edilmemiş |
| **AdminPanel** | Planlandı | ✅ Var (boş) | Scaffolded ama implement edilmemiş |
| **Test Projeleri** | Planlandı | ❌ Yok | Henüz oluşturulmadı |
| **IUnitOfWork** | Planlandı | ✅ Var | Implement edildi |
| **EF Interceptor'lar** | Planlandı | ✅ Var | Auditable + SoftDelete |
| **Tour Configuration** | Yok | ❌ Yok | Tour entity'nin EF config dosyası yok |

### 13.11 Veritabanı — SQL Server 2022

- **Sunucu:** SQL Server 2022 (varsayılan instance, localhost)
- **Veritabanı:** IstGuideDb
- **Kimlik Doğrulama:** Windows Authentication (Trusted_Connection)
- **Tablo Sayısı:** 17 (AspNet* tabloları dahil)
- **Migration Sayısı:** 3
- **Tüm migration'lar başarıyla uygulandı**

### 13.12 Test Edilen ve Çalışan Endpoint'ler

```
✅ GET  /api/v1/guides                          → 200 OK (1 rehber döndü)
✅ GET  /api/v1/guides/{slug}                   → 200 OK (rehber detayı döndü)
✅ GET  /api/v1/districts                       → 200 OK (15 ilçe döndü)
✅ GET  /api/v1/languages                       → 200 OK (12 dil döndü)
✅ GET  /api/v1/specialties                     → 200 OK (10 uzmanlık döndü)
✅ POST /api/v1/contact-requests                → 200 OK (başarıyla oluşturuldu)
✅ POST /api/v1/guides/{id}/reviews (email ile) → 200 OK (başarıyla oluşturuldu)
✅ POST /api/v1/guides/{id}/reviews (emailsiz)  → 200 OK (başarıyla oluşturuldu)
✅ GET  /api/v1/guides/{id}/reviews             → 200 OK
```

### 13.13 Bilinen Sorunlar ve Teknik Borçlar

1. **AutoMapper güvenlik açığı:** v12.0.1'de bilinen bir CVE var (GHSA-rvv3-g6hj-g44x). v13+ geçişte DI uyumsuzluğu yaşanıyor.
2. **Tour entity ilişkisi yok:** Tour şu anda Guide veya District ile ilişkili değil. Faz 2'de ilişkilendirilmeli.
3. **Tour configuration eksik:** Tour entity için EF Core configuration dosyası yok.
4. **Swagger kapalı:** API dokümantasyonu için Swashbuckle eklenmeli.
5. **Test projesi yok:** Unit ve integration testleri yazılmadı.
6. **WebUI ve AdminPanel boş:** React ve Blazor projeleri scaffolded ama implement edilmemiş.
7. **EmailService implementasyonu yok:** Interface tanımlı ama servis yok (Faz 2).
