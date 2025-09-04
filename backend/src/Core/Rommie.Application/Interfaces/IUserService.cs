using System.Net.Cache;
using Rommie.Application.Dtos.Requests;

namespace Rommie.Application.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken = default);
    public Task<(string accessToken, string refreshToken)> LoginUserAsync(string email, string Password, CancellationToken cancellationToken = default);
}
