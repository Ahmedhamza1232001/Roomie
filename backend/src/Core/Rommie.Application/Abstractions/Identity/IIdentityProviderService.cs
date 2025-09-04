using Rommie.Application.Dtos.Responses;

namespace Rommie.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task<LoginUserResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default);
}
