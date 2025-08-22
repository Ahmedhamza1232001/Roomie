using Rommie.Application.Abstractions;
using Rommie.Domain.Entities;
using Rommie.Domain.Entities.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Rommie.Persistence.Data;

public class MerchantDbContext(DbContextOptions<MerchantDbContext> options) : DbContext(options), IUnitOfWork
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
    public virtual DbSet<OutboxConsumerMessage> OutboxConsumerMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Merchant.Persistence.AssemblyRefrence.Assembly);
    }
}
