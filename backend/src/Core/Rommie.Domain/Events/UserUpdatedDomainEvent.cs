using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Events;

public class UserUpdatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public UserUpdatedDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}