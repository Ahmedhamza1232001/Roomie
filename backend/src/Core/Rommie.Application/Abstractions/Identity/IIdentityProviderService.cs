namespace Rommie.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    public Task ToggleTwoFactorAuthenticationAsync(string userIdentitfier, bool enable, CancellationToken cancellationToken = default);
}
