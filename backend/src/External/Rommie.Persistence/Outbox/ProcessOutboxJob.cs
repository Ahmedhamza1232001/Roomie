using System.Data;
using System.Data.Common;
using Dapper;
using Rommie.Application.Abstractions;
using Rommie.Domain.Abstractions;
using Rommie.Domain.Entities.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Rommie.Persistence.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxJob(
    IOptions<OutBoxOptions> options,
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.CreateSqlConnection();
        await using DbTransaction dbTransaction = await dbConnection.BeginTransactionAsync();
        var outbox_messages = await GetOutboxMessagesAsync(dbConnection, dbTransaction);
        logger.LogInformation("Beginning to process outbox messages");

        foreach (OutboxMessage outboxMessage in outbox_messages)
        {
            Exception? exception = null;
            try
            {
                DomainEvent domainEvent = JsonConvert.DeserializeObject<DomainEvent>(
                    outboxMessage.Content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )!;
                using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                IDomainEventDispatcher publisher = serviceScope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();
                await publisher.DispatchAsync(domainEvent);
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
                logger.LogError(exception, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
            }
            finally
            {
                await UpdateOutboxMessage(dbConnection, dbTransaction, outboxMessage, exception);
            }
        }
        await dbTransaction.CommitAsync();
        logger.LogInformation("Completed processing the outbox message ");
    }
    private async Task<IReadOnlyCollection<OutboxMessage>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction dbTransaction)
    {
        string query =
        $"""
        SELECT TOP {options.Value.BatchSize}
        id AS {nameof(OutboxMessage.Id)},
        type AS {nameof(OutboxMessage.Type)},
        content AS {nameof(OutboxMessage.Content)},
        occurred_on_utc AS {nameof(OutboxMessage.OccurredOnUtc)},
        processed_on_utc AS {nameof(OutboxMessage.ProcessedOnUtc)},
        error AS Error
        FROM outbox_messages
        WITH (UPDLOCK, ROWLOCK)
        WHERE processed_on_utc IS NULL
        ORDER BY "occurred_on_utc"
        """;
        var OutboxMessages = await connection.QueryAsync<OutboxMessage>(query, new { }, dbTransaction);
        return [.. OutboxMessages];
    }

    private async Task UpdateOutboxMessage(
        IDbConnection connection,
        IDbTransaction dbTransaction,
        OutboxMessage outboxMessage,
        Exception? exception
    )
    {
        const string query =
        $"""
        UPDATE 
            outbox_messages
        SET
            processed_on_utc = @ProcessedOnUtc ,
            error = @Error
        WHERE id = @Id
        """;

        await connection.ExecuteAsync(query, new
        {
            ProcessedOnUtc = DateTime.UtcNow,
            Error = exception?.Message,
            Id = outboxMessage.Id
        }, dbTransaction);
    }

}

