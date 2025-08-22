namespace Rommie.Domain.Entities.Outbox;

public class OutboxConsumerMessage
{
    public Guid id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
