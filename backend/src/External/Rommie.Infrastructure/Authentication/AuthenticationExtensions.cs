using Rommie.Infrastructure.Authentication.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Rommie.Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<AuthenticationOptionsSetup>();
        services.AddHttpContextAccessor();
        return services;
    }
}
