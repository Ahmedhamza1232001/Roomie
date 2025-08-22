using Rommie.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Rommie.Api.Extensions;

public static class MigrationsExtension
{
    public static void AddMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MerchantDbContext>();
        dbContext.Database.Migrate();
    }
}
