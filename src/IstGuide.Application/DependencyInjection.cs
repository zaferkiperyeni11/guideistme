using AutoMapper;
using FluentValidation;
using IstGuide.Application.Common.Behaviors;
using IstGuide.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IstGuide.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(sp =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(DependencyInjection).Assembly);
            });
            return config.CreateMapper();
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        return services;
    }
}
