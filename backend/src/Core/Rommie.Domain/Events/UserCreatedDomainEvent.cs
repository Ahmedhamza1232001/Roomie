using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserCreatedDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}
