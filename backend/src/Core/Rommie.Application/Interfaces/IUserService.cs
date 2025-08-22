using System.Net.Cache;
using Rommie.Application.Dtos.Requests;

namespace Rommie.Application.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUser(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken = default);

    public Task UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserRequest, CancellationToken cancellationToken = default);

    public Task<bool> ToggleTwoFactorAuthenticationAsync(Guid userId, CancellationToken cancellationToken = default);
}
