using Dapper;
using Rommie.Application.Abstractions;
using Rommie.Domain.Abstractions;
using Rommie.Domain.Entities.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Rommie.Persistence.Outbox;

public class OutboxIdempotentDomainEventHandlerDecorator<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> innerHandler,
    IDbConnectionFactory dbConnectionFactory,
    ILogger<OutboxIdempotentDomainEventHandlerDecorator<TDomainEvent>> logger) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task HandleAsync(TDomainEvent notification, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateSqlConnection();

        IDomainEvent domainEvent = notification;
        var outboxConsumerMessage = new OutboxConsumerMessage
        {
            id = domainEvent.Id,
            HandlerName = innerHandler.GetType().Name
        };

        const string query = $"SELECT COUNT(1) FROM outbox_consumer_messages WHERE id = @id AND handler_name = @HandlerName";
        var exists = await connection.ExecuteScalarAsync<int>(query, outboxConsumerMessage);

        if (exists > 0)
        {
            logger.LogWarning("Duplicate event detected: {EventType} with ID {EventId}. Skipping processing.", typeof(TDomainEvent).Name, outboxConsumerMessage.id);
            return;
        }

        logger.LogInformation("Processing event {EventType} with ID {EventId}.", typeof(TDomainEvent).Name, outboxConsumerMessage.id);
        await innerHandler.HandleAsync(notification, cancellationToken);

        const string insertQuery = $"INSERT INTO outbox_consumer_messages (id, handler_name) VALUES (@Id, @HandlerName)";
        await connection.ExecuteAsync(insertQuery, outboxConsumerMessage);

        logger.LogInformation("Stored event {EventType} with ID {EventId} in OutboxConsumerMessages.", typeof(TDomainEvent).Name, outboxConsumerMessage.id);

    }

}