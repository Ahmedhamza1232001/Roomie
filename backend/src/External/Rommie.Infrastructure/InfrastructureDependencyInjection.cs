using Rommie.Application.Abstractions.Identity;
using Rommie.Infrastructure.Authentication;
using Rommie.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Rommie.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeyCloakOptions>(configuration.GetSection("KeyCloak"));
        services.AddTransient<AdminKeyCloakAuthDelegatingHandler>();
        services.AddHttpClient<AdminKeyCloakClient>((sp, client) =>
        {
            KeyCloakOptions options = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            client.BaseAddress = new Uri(options.AdminUrl);
        })
        .AddHttpMessageHandler<AdminKeyCloakAuthDelegatingHandler>();
        services.AddHttpClient<TokenKeyCloackCLient>((sp, client) =>
        {
            KeyCloakOptions options = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            client.BaseAddress = new Uri(options.TokenUrl);
        });
        services.AddTransient<IIdentityProviderService, IdentityProviderService>();
        services.AddAuthenticationInternal();
        return services;
    }
}
