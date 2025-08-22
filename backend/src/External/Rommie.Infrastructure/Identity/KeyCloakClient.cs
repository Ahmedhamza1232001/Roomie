using System.Net.Http.Json;
using Rommie.Infrastructure.Identity.Reporesentations;

namespace Rommie.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    internal async Task AddRolesToUserAsync(string userIdentifier, ICollection<RoleRepresentation> roleRepresentations, CancellationToken cancellationToken = default)
    {
        var requestUri = $"users/{userIdentifier}/role-mappings/realm";
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            requestUri,
            roleRepresentations,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    internal async Task RemoveRolesFromUserAsync(string userIdentifier, ICollection<RoleRepresentation> roleRepresentations, CancellationToken cancellationToken = default)
    {
        var requestUri = $"users/{userIdentifier}/role-mappings/realm";

        // Create the JSON content manually
        var content = JsonContent.Create(roleRepresentations);

        // Construct DELETE request with body
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = content
        };

        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
