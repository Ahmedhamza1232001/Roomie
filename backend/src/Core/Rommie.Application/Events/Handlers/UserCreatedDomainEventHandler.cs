using Rommie.Application.Abstractions;
using Rommie.Domain.Abstractions;
using Rommie.Domain.Entities;
using Rommie.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Rommie.Application.Events.Handlers;

public class UserCreatedDomainEventHandler(IGenericRepository<User, Guid> userGenericRepository, ILogger<UserCreatedDomainEventHandler> logger) : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var user = await userGenericRepository.GetById(domainEvent.UserId);
        if (user == null)
        {
            logger.LogError("User with ID {UserId} not found for UserCreatedDomainEvent", domainEvent.UserId);
            throw new InvalidOperationException($"User with ID {domainEvent.UserId} not found.");
        }
        logger.LogInformation("User created with ID: {UserId}, Email: {Email}", domainEvent.UserId, user.Email);
    }
}
