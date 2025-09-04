using System.Net;
using Rommie.Application.Abstractions.Identity;
using Rommie.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Rommie.Infrastructure.Identity.Representations;
using Rommie.Infrastructure.Identity.Representations.Requests;

namespace Rommie.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    // POST /admin/realms/{realm}/users
    public async Task<string> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
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
}
