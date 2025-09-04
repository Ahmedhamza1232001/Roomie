using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Rommie.Infrastructure.Identity.Representations.Responses;

namespace Rommie.Infrastructure.Identity
{
    internal class TokenKeyCloackCLient(HttpClient httpClient, IOptions<KeyCloakOptions> options)
    {
        private readonly KeyCloakOptions _options = options.Value;
        internal async Task<LoginResponseRepresentation> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var loginRepresentation = new KeyValuePair<string, string>[]
            {
            new ("client_id", _options.ConfidentialClientId),
            new ("username", email),
            new ("password", password),
            new ("client_secret", _options.ConfidentialClientSecret),
            new ("grant_type", "password"),
            new ("scope", "openid email")
            };

            var authRequestContent = new FormUrlEncodedContent(loginRepresentation);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(
                "",
                authRequestContent,
                cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseRepresentation>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Failed to read authorization token from response.");
        }
    }
}