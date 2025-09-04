using System.Net.Cache;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Dtos.Responses;

namespace Rommie.Application.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken = default);
    public Task<LoginUserResponse> LoginUserAsync(string email, string Password, CancellationToken cancellationToken = default);
}
