namespace Rommie.Persistence.Outbox;

public class OutBoxOptions
{
    public int BatchSize { get; set; }
    public int TimeSpanInSeconds { get; set; }
}
