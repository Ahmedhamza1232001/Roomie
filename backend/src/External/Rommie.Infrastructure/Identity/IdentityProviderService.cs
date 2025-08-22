using System.Net;
using Rommie.Application.Abstractions.Identity;
using Rommie.Domain.Exceptions;
using Rommie.Infrastructure.Identity.Reporesentations;
using Microsoft.Extensions.Logging;

namespace Rommie.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    // POST /admin/realms/{realm}/users
    public async Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");
            throw new ConflictException("User.Conflict.Email", user.Email);
        }
    }
    public Task ToggleTwoFactorAuthenticationAsync(string userIdentitfier, bool enable, CancellationToken cancellationToken = default)
    {
        ICollection<RoleRepresentation> userRolesRepresentations = [new RoleRepresentation("b0f9fabe-8199-40bb-99e0-950a65727b99", "tfa_enabled")];
        if (enable)
        {
            return keyCloakClient.AddRolesToUserAsync(userIdentitfier, userRolesRepresentations, cancellationToken);
        }
        else
        {
            return keyCloakClient.RemoveRolesFromUserAsync(userIdentitfier, userRolesRepresentations, cancellationToken);
        }
    }
}
