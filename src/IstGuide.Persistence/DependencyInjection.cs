using IstGuide.Domain.Common;
using IstGuide.Domain.Repositories;
using IstGuide.Infrastructure.Identity;
using IstGuide.Persistence.Interceptors;
using IstGuide.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IstGuide.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>();

            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            options.AddInterceptors(auditInterceptor, softDeleteInterceptor);
        });

        services.AddScoped<IstGuide.Application.Common.Interfaces.IApplicationDbContext>(
            sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGuideRepository, GuideRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IContactRequestRepository, ContactRequestRepository>();

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
        .AddDefaultTokenProviders();

        return services;
    }
}
