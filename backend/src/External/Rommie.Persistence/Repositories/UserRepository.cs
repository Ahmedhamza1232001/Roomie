using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rommie.Application.Repositories;
using Rommie.Domain.Entities;
using Rommie.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Rommie.Persistence.Repositories
{
    public class UserRepository(MerchantDbContext merchantDbContext) : IUserRepository
    {
        public Task<User?> GetByEmail(string email)
        {
            return merchantDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}