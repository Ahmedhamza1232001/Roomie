using Rommie.Domain.Abstractions;

namespace Rommie.Application.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

}