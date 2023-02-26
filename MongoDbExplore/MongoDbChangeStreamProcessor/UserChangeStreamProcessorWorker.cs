using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbRepository.EntityModel;

namespace MongoDbChangeStreamProcessor;

public class UserChangeStreamProcessorWorker : BackgroundService
{
    private readonly ILogger<UserChangeStreamProcessorWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _connectionString;
    private readonly string _databaseName;

    public UserChangeStreamProcessorWorker(
        ILogger<UserChangeStreamProcessorWorker> logger, 
        IServiceProvider serviceProvider,
        IOptions<MongoDbConfiguration> mongoOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _connectionString = mongoOptions.Value.ConnectionString
            ?? throw new InvalidDataException("Connection string configuration not found");

        _databaseName = mongoOptions.Value.DatabaseName
            ?? throw new InvalidDataException("Database configuration not found");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
                var database = mongoClient.GetDatabase(_databaseName);

                var watcher = scope.ServiceProvider.GetRequiredService<IChangeStreamObserver<User>>();
                //await watcher.ObserveCollection(_connectionString, _databaseName, stoppingToken);
                await watcher.ObserveCollection(database, stoppingToken);
            }

            Thread.Sleep(5000); // Maybe move to config
        }
    }
}
