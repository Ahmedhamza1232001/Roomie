using System.Xml.XPath;
using Rommie.Application.Abstractions;
using Rommie.Application.Repositories;
using Rommie.Domain.Abstractions;
using Rommie.Persistence.Data;
using Rommie.Persistence.Outbox;
using Rommie.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

namespace Rommie.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("TestConnection")!;
        services.AddDbContext<MerchantDbContext>((sp, options) =>
        {
            options
                .UseSqlServer(dbConnectionString, op => op.MigrationsAssembly(Rommie.Persistence.AssemblyRefrence.Assembly))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });
        services.Decorate(typeof(IDomainEventHandler<>), typeof(OutboxIdempotentDomainEventHandlerDecorator<>));
        services.TryAddSingleton<PublishOutboxMessagesInterceptor>();
        services.Configure<OutBoxOptions>(configuration.GetSection("OutBox"));
        services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<MerchantDbContext>());
        services.AddScoped<IDbConnectionFactory>(x => new DbConnectionFactory(dbConnectionString));
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        // adding quartz for background jobs 
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        return services;
    }
}
