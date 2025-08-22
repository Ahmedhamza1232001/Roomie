using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rommie.Domain.Abstractions;
using Rommie.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Rommie.Application.Projections
{
    public class UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger) : IDomainEventHandler<UserCreatedDomainEvent>
    {
        public Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(
                "UserCreatedDomainEventHandler: User with ID {UserId} created at {CreationTime}",
                domainEvent.UserId,
                domainEvent.CreatedOnUtc
            );
            return Task.CompletedTask;
        }
    }
}