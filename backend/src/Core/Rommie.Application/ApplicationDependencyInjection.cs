using FluentValidation;
using FluentValidation.AspNetCore;
using Rommie.Application.Abstractions;
using Rommie.Application.Events;
using Rommie.Application.Events.Handlers;
using Rommie.Application.Interfaces;
using Rommie.Application.Services;
using Rommie.Domain.Abstractions;
using Rommie.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Rommie.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Rommie.Application.AssemblyRefrence.Assembly);
        // Register the domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUserService, UserService>();
        // Register domain event handlers
        services.Scan(scan => scan
            .FromAssemblies(Rommie.Application.AssemblyRefrence.Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        return services;
    }
}
