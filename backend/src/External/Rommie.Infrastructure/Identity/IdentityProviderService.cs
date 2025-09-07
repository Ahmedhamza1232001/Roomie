using System.Net;
using Rommie.Application.Abstractions.Identity;
using Rommie.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Rommie.Infrastructure.Identity.Representations;
using Rommie.Infrastructure.Identity.Representations.Requests;
using Microsoft.Extensions.Options;
using Rommie.Application.Dtos.Responses;

namespace Rommie.Infrastructure.Identity;

internal sealed class IdentityProviderService(AdminKeyCloakClient adminKeyCloakClient, TokenKeyCloackCLient tokenKeyCloackCLient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    // POST /admin/realms/{realm}/users
    public async Task<LoginUserResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var authResponse = await tokenKeyCloackCLient.LoginUserAsync(email, password, cancellationToken);
        return new LoginUserResponse()
        {
            AccessToken = authResponse.AccessToken,
            RefreshToken = authResponse.RefreshToken
        };
    }

    public Task<LoginUserResponse> RefreshUserAsync(string token, CancellationToken cancellationToken = default)
    {
        var authResponse = tokenKeyCloackCLient.RefreshTokenAsync(token, cancellationToken);
        return Task.FromResult(new LoginUserResponse()
        {
            AccessToken = authResponse.Result.AccessToken,
            RefreshToken = authResponse.Result.RefreshToken
        });
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
            [new CredentialRepresentation("password", user.Password, false)]);

        try
        {
            string identityId = await adminKeyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError("User registration failed {exception}", exception);
            throw new ConflictException("User.Conflict.Email", user.Email);
        }
    }
}
