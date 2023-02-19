namespace SerilogConfigurationSample.Services;

public interface ISampleService
{
    void Test();
}

public class SampleService : ISampleService
{
    private readonly ILogger<SampleService> _logger;
    private readonly CorelationIdProvider _corelationIdProvider;

    public SampleService(ILogger<SampleService> logger, CorelationIdProvider corelationIdProvider)
	{
        _logger = logger;
        _corelationIdProvider = corelationIdProvider;
    }

    public void Test()
    {
        _logger.LogInformation("Sample Service {CorelationId}", _corelationIdProvider.CorelationId);
    }
}
