namespace SerilogConfigurationSample;

public sealed class CorelationIdProvider
{
    public string CorelationId { get; }

	public CorelationIdProvider()
	{
		CorelationId = Guid.NewGuid().ToString("N");
	}
}
