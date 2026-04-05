using IstGuide.Application.Common.Interfaces;
using IstGuide.Infrastructure.Identity;
using IstGuide.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IstGuide.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ISlugService, SlugService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IWhatsAppService, WhatsAppService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IIdentityService, IstGuide.Infrastructure.Identity.IdentityService>();

        return services;
    }
}
