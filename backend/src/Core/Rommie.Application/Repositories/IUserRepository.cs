using Rommie.Domain.Entities;

namespace Rommie.Application.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByEmail(string email);
}
