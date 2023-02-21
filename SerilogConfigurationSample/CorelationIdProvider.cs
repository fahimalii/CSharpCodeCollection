namespace SerilogConfigurationSample;

public record class CorelationIdProvider
{
    public string CorelationId { get; }

	public CorelationIdProvider()
	{
		CorelationId = Guid.NewGuid().ToString("N");
	}
}
