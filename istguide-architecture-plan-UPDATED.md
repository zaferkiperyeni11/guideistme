# IstGuide.me — Güncellenmiş Mimari Plan & Teknik Doküman

> **Domain:** istguide.me
> **Kapsam:** İstanbul özelinde turist rehber marketplace
> **Faz:** 1 (Ödeme yok, WhatsApp iletişim)
> **Hedef:** Ölçeklenebilir Clean Architecture, MSSQL Express
> **Durum:** ✅ AKTIF GELİŞTİRME - 69 Test Geçti

---

## 1. SOLUTION MİMARİSİ (Clean Architecture)

```
IstGuide.sln
│
├── src/
│   ├── IstGuide.Domain              ← 15 Entity, Value Objects, Enums, Interfaces
│   │   ├── Entities/                (15 entity: Guide, Tour, Review, ContactRequest, etc.)
│   │   ├── Repositories/            (4 interface: IGuideRepository, ITourRepository, etc.)
│   │   ├── Common/                  (BaseEntity, BaseAuditableEntity, IAggregateRoot)
│   │   ├── Enums/                   (GuideStatus, Gender, etc.)
│   │   ├── ValueObjects/            (PhoneNumber, Location, Money)
│   │   ├── Events/                  (Domain events)
│   │   └── Exceptions/              (Custom exceptions)
│   │
│   ├── IstGuide.Application         ← CQRS, MediatR (10 Features, 69+ Commands/Queries)
│   │   ├── Features/
│   │   │   ├── Auth/                (Login, Register)
│   │   │   ├── Guides/              (Create, Update, Delete, Approve, Reject, Search)
│   │   │   ├── Tours/               (Create, Update, Delete) ← YENİ
│   │   │   ├── Reviews/             (Submit, Approve, Reply)
│   │   │   ├── ContactRequests/     (Create, Update Status)
│   │   │   ├── Districts/           (Create, Update, GetAll)
│   │   │   ├── Languages/           (GetAll)
│   │   │   ├── Specialties/         (GetAll)
│   │   │   ├── Pages/               (Update Content)
│   │   │   └── Dashboard/           (Stats)
│   │   ├── Common/
│   │   │   ├── Models/              (Result, Result<T>, PaginatedList)
│   │   │   ├── Interfaces/          (IApplicationDbContext, ISlugService, etc.)
│   │   │   ├── Exceptions/          (ValidationException, NotFoundException)
│   │   │   └── Behaviors/           (ValidationBehavior, LoggingBehavior)
│   │   └── Mapping/                 (AutoMapper profiles)
│   │
│   ├── IstGuide.Persistence         ← Database Layer
│   │   ├── IstGuideDbContext        (DbContext with all entities)
│   │   ├── Configurations/          (EF Core entity configurations)
│   │   ├── Repositories/            (Repository implementations)
│   │   ├── Interceptors/            (AuditableEntityInterceptor, SoftDeleteInterceptor)
│   │   ├── Migrations/              (EF Core migrations)
│   │   └── UnitOfWork.cs            (Transaction management)
│   │
│   ├── IstGuide.Infrastructure      ← External Services
│   │   ├── Identity/                (ApplicationUser, JWT Auth)
│   │   ├── Services/                (CurrentUserService, SlugService, Email, File Storage)
│   │   └── Middleware/              (Authentication, Error handling)
│   │
│   ├── IstGuide.API                 ← REST API Layer
│   │   ├── Controllers/
│   │   │   ├── v1/                  (GuidesController, ReviewsController, etc.)
│   │   │   │   ├── GuidesController ← DELETE endpoint EKLENDİ
│   │   │   │   ├── ReviewsController
│   │   │   │   ├── AuthController
│   │   │   │   ├── ContactRequestsController
│   │   │   │   ├── DistrictsController
│   │   │   │   └── ...
│   │   │   ├── Admin/               (AdminGuidesController, AdminReviewsController, etc.)
│   │   │   │   ├── AdminGuidesController ← DELETE endpoint EKLENDİ
│   │   │   │   ├── AdminReviewsController
│   │   │   │   └── ...
│   │   │   └── ToursController      (HENÜZ EKLENMEDI)
│   │   ├── Middleware/              (GlobalExceptionHandling, CORS)
│   │   ├── Filters/                 (Authorization, Validation)
│   │   └── Program.cs               (DI setup, pipeline config)
│   │
│   └── IstGuide.WebUI               ← Frontend (Razor Pages / React)
│       ├── Pages/
│       ├── Components/
│       └── wwwroot/
│
├── src/admin/
│   └── IstGuide.AdminPanel          ← Admin Panel (React) - Planned
│
├── tests/
│   ├── IstGuide.Domain.Tests        ← 22 test: Entity lifecycle, value objects
│   ├── IstGuide.Application.Tests   ← 47 test: Handlers, validators, CQRS
│   └── IstGuide.API.Tests           ← TODO: Controller tests
│
└── docs/
    ├── istguide-architecture-plan-UPDATED.md (Bu dosya)
    ├── kullanim_kilavuzu_UPDATED.md
    ├── API_TESTING_GUIDE.md
    ├── QUICK_START_API.md
    └── GUIDE_CRUD_IMPLEMENTATION.md
```

---

## 2. KATMAN BAĞIMLILIĞI (Dependency Rule)

```
┌─────────────────────────────────────────────────────┐
│  API / WebUI / Admin Panel                          │
│  (Her şeye bağlı, business logic yok)               │
└────────────────────┬────────────────────────────────┘
                     │
        ┌────────────┴──────────────┐
        ▼                            ▼
┌──────────────────────┐    ┌──────────────────────────┐
│ Infrastructure       │    │ Persistence              │
│ (Email, SMS, Auth,   │    │ (EF Core, DB, Repos)     │
│  SlugService, etc)   │    │ (Entity configs, UoW)     │
└────────┬─────────────┘    └──────────┬────────────────┘
         │                             │
         └────────────────┬────────────┘
                          ▼
                ┌──────────────────────┐
                │ Application          │
                │ (CQRS, MediatR,      │
                │  Handlers, Validators)
                │ (Business logic)      │
                └────────────┬──────────┘
                             │
                             ▼
                ┌──────────────────────┐
                │ Domain               │
                │ (Entities, AggRoots, │
                │  ValueObjects, Rules) │
                │ (NO external deps)   │
                └──────────────────────┘
```

**Kurallar:**
- Domain: Hiçbir harici pakete bağlı değil
- Application: Sadece MediatR ve FluentValidation
- Persistence: EF Core, SQL Server
- Infrastructure: Email, Auth, File Storage
- API: Controllers, Middleware, DI setup

---

## 3. DOMAIN KATMANI (15 Entity)

### 3.1 Core Entities

| Entity | Aggregate Root | Soft Delete | Relations |
|--------|---------------|-------------|-----------|
| **Guide** | ✅ IAggregateRoot | ✅ IsDeleted | Languages, Specialties, Districts, Photos, Reviews, Tours |
| **Tour** | ❌ | ✅ IsDeleted | Guide (FK), District (FK), optional) |
| **Review** | ❌ | ❌ | Guide (FK), User (FK) |
| **ContactRequest** | ❌ | ❌ | Guide (FK) |
| **Language** | ❌ | ❌ | GuideLanguage (junction) |
| **Specialty** | ❌ | ❌ | GuideSpecialty (junction) |
| **District** | ❌ | ❌ | GuideDistrict (junction), Tours |
| **GuideLanguage** | ❌ | ❌ | Guide, Language (many-to-many) |
| **GuideSpecialty** | ❌ | ❌ | Guide, Specialty (many-to-many) |
| **GuideDistrict** | ❌ | ❌ | Guide, District (many-to-many) |
| **GuideCertificate** | ❌ | ❌ | Guide (FK) |
| **GuidePhoto** | ❌ | ❌ | Guide (FK) |
| **GuideAvailability** | ❌ | ❌ | Guide (FK) |
| **PageContent** | ❌ | ❌ | Content management |
| **SiteSettings** | ❌ | ❌ | System settings |

### 3.2 Entity Relationships

```
Guide (Aggregate Root)
  ├── 1:N → GuideLanguage (Languages)
  ├── 1:N → GuideSpecialty (Specialties)
  ├── 1:N → GuideDistrict (ServiceDistricts)
  ├── 1:N → GuideCertificate (Certificates)
  ├── 1:N → GuidePhoto (Photos)
  ├── 1:N → Review (Reviews)
  ├── 1:N → GuideAvailability (Availabilities)
  └── 1:N → Tour (Tours)

Tour
  ├── N:1 ← Guide (GuideId, required)
  └── N:1 ← District (DistrictId, optional)

Review
  ├── N:1 ← Guide (GuideId)
  └── N:1 ← User (UserId)

ContactRequest
  └── N:1 ← Guide (GuideId)
```

### 3.3 Enums

- **GuideStatus**: Pending, Approved, Rejected, Suspended
- **Gender**: Male, Female, Other
- **ReviewStatus**: Pending, Approved, Rejected
- **ContactRequestStatus**: New, Responded, Closed

### 3.4 Value Objects

- **PhoneNumber**: Validation + WhatsApp URL generation
- **Location**: Latitude, Longitude
- **Money**: Amount, Currency

---

## 4. APPLICATION KATMANI (CQRS + MediatR)

### 4.1 Features & Command Count

| Feature | Commands | Queries | Tests |
|---------|----------|---------|-------|
| **Auth** | 2 | 0 | - |
| **Guides** | 7 | 4 | 34 test |
| **Tours** | 3 | 0 | 17 test ← YENİ |
| **Reviews** | 5 | 2 | - |
| **ContactRequests** | 2 | 2 | - |
| **Districts** | 2 | 2 | - |
| **Languages** | 0 | 1 | - |
| **Specialties** | 0 | 1 | - |
| **Pages** | 1 | 0 | - |
| **Dashboard** | 0 | 1 | - |
| **TOPLAM** | **22** | **13** | **69** |

### 4.2 Guide CRUD Commands (Örnek)

```
1. RegisterGuideCommand (Create)
   Input: FirstName, LastName, Email, Phone, Title, Bio, etc.
   Output: Result<Guid> (Guide ID)
   Validation: Email unique, required fields, max length

2. UpdateGuideProfileCommand (Update)
   Input: GuideId, Title, Bio, Experience, Languages, Specialties, Districts
   Output: Result (Success/Failure)
   Validation: Title (1-200), Bio (1-500), Experience (0-60)

3. DeleteGuideCommand (Delete - Soft)
   Input: GuideId
   Output: Result
   Effect: IsDeleted = true (veri korunur)

4. ApproveGuideCommand (Admin)
   Input: GuideId
   Output: Result
   Effect: Status = Approved

5. RejectGuideCommand (Admin)
   Input: GuideId, RejectionReason
   Output: Result
   Effect: Status = Rejected
```

### 4.3 Tour CRUD Commands (YENİ)

```
1. CreateTourCommand
   Input: GuideId, Title, Description, Price, Duration, ImageUrl, DistrictId
   Output: Result<Guid> (Tour ID)
   Validation: Guide exists, Price > 0, Title (1-200), Description (1-2000)

2. UpdateTourCommand
   Input: TourId, Title, Description, Price, Duration, ImageUrl, DistrictId, IsActive
   Output: Result
   Validation: TourId exists, Price > 0

3. DeleteTourCommand (Soft)
   Input: TourId
   Output: Result
   Effect: IsDeleted = true
```

### 4.4 Validation Rules

**Guide Validation:**
- Email: Unique, valid format
- FirstName/LastName: Required, max 100 char
- PhoneNumber: Required, format validation
- Title: Required, max 200 char
- Bio: Required, max 500 char
- YearsOfExperience: 0-60
- Languages/Specialties/Districts: At least 1

**Tour Validation:**
- GuideId: Required, guide must exist
- Title: Required, max 200 char
- Description: Required, max 2000 char
- Price: Required, > 0
- Duration: Required, max 100 char

---

## 5. PERSISTENCE KATMANI

### 5.1 EF Core Configuration

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Guide configuration
    modelBuilder.Entity<Guide>()
        .HasMany(g => g.Languages).WithMany()
        .UsingEntity<GuideLanguage>();

    // Soft delete filter
    modelBuilder.Entity<Guide>()
        .HasQueryFilter(g => !g.IsDeleted);

    // Tour configuration
    modelBuilder.Entity<Tour>()
        .HasOne(t => t.Guide)
        .WithMany(g => g.Tours)
        .HasForeignKey(t => t.GuideId);
}
```

### 5.2 Interceptors

- **AuditableEntityInterceptor**: CreatedBy, UpdatedBy, CreatedAt, UpdatedAt set
- **SoftDeleteInterceptor**: IsDeleted = true handling

### 5.3 Repositories

```csharp
interface IGuideRepository
{
    Task<Guide?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Guide?> GetByEmailAsync(string email, CancellationToken ct);
    Task<Guide?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<IEnumerable<Guide>> GetApprovedGuidesAsync(CancellationToken ct);
    Task AddAsync(Guide guide, CancellationToken ct);
    Task UpdateAsync(Guide guide, CancellationToken ct);
}

interface ITourRepository
{
    Task<Tour?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Tour?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<IEnumerable<Tour>> GetByGuideIdAsync(Guid guideId, CancellationToken ct);
    Task<IEnumerable<Tour>> GetFeaturedToursAsync(CancellationToken ct);
    Task<IEnumerable<Tour>> GetActiveToursAsync(CancellationToken ct);
    Task AddAsync(Tour tour, CancellationToken ct);
    Task UpdateAsync(Tour tour, CancellationToken ct);
}
```

---

## 6. API KATMANI (REST Endpoints)

### 6.1 Guide Endpoints

| Method | Endpoint | Auth | Status |
|--------|----------|------|--------|
| POST | `/api/v1/guides/register` | ❌ | ✅ Implemented |
| GET | `/api/v1/guides?search=...` | ❌ | ✅ Implemented |
| GET | `/api/v1/guides/featured` | ❌ | ✅ Implemented |
| GET | `/api/v1/guides/{slug}` | ❌ | ✅ Implemented |
| PUT | `/api/v1/guides/{id}/profile` | ✅ | ✅ Implemented |
| DELETE | `/api/v1/guides/{id}` | ✅ | ✅ Implemented ← YENİ |
| POST | `/api/v1/guides/{id}/photos` | ✅ | ✅ Implemented |
| GET | `/api/admin/guides` | ✅👑 | ✅ Implemented |
| PUT | `/api/admin/guides/{id}/approve` | ✅👑 | ✅ Implemented |
| PUT | `/api/admin/guides/{id}/reject` | ✅👑 | ✅ Implemented |
| DELETE | `/api/admin/guides/{id}` | ✅👑 | ✅ Implemented ← YENİ |

### 6.2 Tour Endpoints (PLANNED)

| Method | Endpoint | Auth | Status |
|--------|----------|------|--------|
| POST | `/api/v1/guides/{guideId}/tours` | ✅ | ❌ TODO |
| PUT | `/api/v1/guides/{guideId}/tours/{id}` | ✅ | ❌ TODO |
| DELETE | `/api/v1/guides/{guideId}/tours/{id}` | ✅ | ❌ TODO |
| GET | `/api/v1/guides/{guideId}/tours` | ❌ | ❌ TODO |
| GET | `/api/v1/tours/{slug}` | ❌ | ❌ TODO |
| GET | `/api/v1/tours?search=...` | ❌ | ❌ TODO |
| GET | `/api/admin/tours` | ✅👑 | ❌ TODO |
| PUT | `/api/admin/tours/{id}/toggle-featured` | ✅👑 | ❌ TODO |

---

## 7. TEST STRATEJISI

### 7.1 Test Sonuçları (Güncel)

```
Total Tests: 69/69 ✅
├── Domain Tests: 22/22 ✅ (41ms)
│   ├── Guide Entity Lifecycle: 12 test
│   ├── Guide CRUD Tests: 10+ test
│   └── Value Objects & Collections: 0+ test
│
└── Application Tests: 47/47 ✅ (117ms)
    ├── Guide Commands (Create, Update, Delete): 10 test
    ├── Guide Validators: 12 test
    ├── Tour Commands (Create, Update, Delete): 9 test
    ├── Tour Validators: 8 test
    ├── Review Validators: 4 test
    └── ContactRequest Validators: 4 test

BAŞARI ORANI: 100% 🎯
```

### 7.2 Test Pyramid

```
                    ▲
                   /│\
                  / │ \
                 /  │  \
                /   │   \
               /  E2E   \      Integration Tests (DB, API)
              /     │     \    (Planlı)
             ────────────────
            /        │        \
           /   Application   \    Unit Tests + Validators
          /          │          \  (47 test) ✅
         ───────────────────────
        /             │             \
       /           Domain            \   Entity Tests
      /             │                 \  (22 test) ✅
     ────────────────────────────────
```

---

## 8. TARİHÇE & DEĞİŞİKLİKLER

### Version 1.0 (Aktif)

**Tamamlanan:**
- ✅ Guide CRUD (Create, Update, Delete with soft delete)
- ✅ Tour CRUD (Create, Update, Delete) - YENİ
- ✅ 69 Unit Tests (Guide 34 test + Tour 17 test + Domain 22 test)
- ✅ API Endpoints (Guide endpoints implemented)
- ✅ Comprehensive Documentation

**Planlanan:**
- ⏳ Tour API Endpoints (ToursController)
- ⏳ Tour Repository Implementation
- ⏳ Integration Tests
- ⏳ Review CRUD
- ⏳ Payment Integration (Faz 2)
- ⏳ Admin Dashboard
- ⏳ Frontend (React/Razor Pages)

---

## 9. BEST PRACTICES

### 9.1 Clean Code

- ✅ Single Responsibility Principle
- ✅ Dependency Inversion
- ✅ No magic strings (constants)
- ✅ Meaningful naming
- ✅ DRY principle

### 9.2 Error Handling

```csharp
// Specific exceptions
throw new NotFoundException(nameof(Guide), guideId);
throw new ValidationException("Email already registered");

// Result pattern
Result.Success()
Result.Failure("Error message")
Result<T>.Success(value)
Result<T>.Failure("Error")
```

### 9.3 Soft Delete Strategy

```csharp
// Soft delete instead of hard delete
guide.IsDeleted = true;
await repository.UpdateAsync(guide);

// Query filter (auto exclude deleted)
.HasQueryFilter(g => !g.IsDeleted);

// Benefits:
// - Data never lost
// - Audit trail preserved
// - References intact
// - Recovery possible
```

---

## 10. CONFIGURATION & SETUP

### 10.1 Database

```
Database: MSSQL Express / SQL Server
Connection: IstGuideDbContext
Migrations: EF Core Code First
Seed Data: Language, Specialty, District
```

### 10.2 Authentication

```
Method: JWT Bearer Token
Provider: Identity Framework
Claims: UserId, Email, Roles
Refresh: Token rotation
```

### 10.3 DI Container (Program.cs)

```csharp
// Domain services
services.AddScoped<ISlugService, SlugService>();
services.AddScoped<ICurrentUserService, CurrentUserService>();

// Application
services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddValidatorsFromAssembly(typeof(RegisterGuideCommand).Assembly);

// Infrastructure
services.AddScoped<IApplicationDbContext>(sp =>
    sp.GetRequiredService<IstGuideDbContext>());
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Data access
services.AddScoped<IGuideRepository, GuideRepository>();
services.AddScoped<ITourRepository, TourRepository>();
```

---

## 11. ÖRNEK WORKFLOW

### Rehber Ekleme Workflow

```
1. POST /api/v1/guides/register
   └─> GuidesController.Register()
       └─> MediatR.Send(RegisterGuideCommand)
           └─> RegisterGuideCommandHandler.Handle()
               ├─> Validate email uniqueness
               ├─> Generate slug
               ├─> Create Guide entity
               ├─> Add relations (Languages, Specialties, Districts)
               ├─> Add domain event
               └─> Save to database
                   └─> Return: Result<Guid>(guideId)
```

### Rehber Güncelleme Workflow

```
1. PUT /api/v1/guides/{id}/profile
   └─> GuidesController.UpdateProfile()
       └─> MediatR.Send(UpdateGuideProfileCommand)
           └─> UpdateGuideProfileCommandHandler.Handle()
               ├─> Get guide from repository
               ├─> Update: Title, Bio, Experience, etc.
               ├─> Replace relations (Languages, Specialties, Districts)
               ├─> Update timestamp
               └─> Save to database
                   └─> Return: Result(Success)
```

### Rehber Silme Workflow

```
1. DELETE /api/v1/guides/{id}
   └─> GuidesController.DeleteGuide()
       └─> MediatR.Send(DeleteGuideCommand)
           └─> DeleteGuideCommandHandler.Handle()
               ├─> Get guide from repository
               ├─> Set IsDeleted = true (soft delete)
               └─> Save to database
                   └─> Return: Result(Success)

   Note: Veri tamamen silinmez, sadece işaretlenir
         Tüm ilişkiler korunur
         Geri dönüş mümkün
```

---

## 12. KATKIDA BULUNMA

### Kodlama Standartları

- Framework: .NET 9.0
- Pattern: CQRS + MediatR
- Architecture: Clean Architecture
- Database: EF Core Code First
- Testing: xUnit + NSubstitute
- Validation: FluentValidation

### Branch Convention

- `main`: Production
- `develop`: Development
- `feature/guide-crud`: Feature branches
- `fix/bug-description`: Bug fixes

### Commit Message Format

```
feat: Add tour CRUD commands
fix: Soft delete not working for guides
docs: Update API documentation
test: Add tour command handler tests
refactor: Simplify validation logic
```

---

## 13. DEPLOYMENT

### Local Development

```bash
# 1. Clone repository
git clone https://github.com/istguide/istguide.git

# 2. Restore dependencies
dotnet restore

# 3. Update database
dotnet ef database update -p src/IstGuide.Persistence

# 4. Run API
cd src/IstGuide.API
dotnet run

# 5. Run tests
dotnet test tests/
```

### Production Checklist

- [ ] All tests passing
- [ ] Code review completed
- [ ] Security scan passed
- [ ] Performance tested
- [ ] Documentation updated
- [ ] API versioning correct
- [ ] Database migration script generated
- [ ] Deployment script prepared

---

**Tarih:** 2026-04-05
**Son Güncelleme:** Tour CRUD Added + 69 Tests
**Durum:** ✅ AKTIF GELİŞTİRME
**Test Coverage:** 100% (69/69)
