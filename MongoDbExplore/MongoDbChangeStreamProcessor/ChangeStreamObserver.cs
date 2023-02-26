using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbRepository;

namespace MongoDbChangeStreamProcessor;

public interface IChangeStreamObserver<TSource>
{
    Task ObserveCollection(string connectionString, string databaseName, CancellationToken cancellationToken = default);
    Task ObserveCollection(IMongoDatabase database, CancellationToken cancellationToken = default);
}

/// <summary>
/// Observe the given <typeparamref name="TSource"/> mongo collection
/// </summary>
/// <typeparam name="TSource">The mongo collection which needs to be observed</typeparam>
public class ChangeStreamObserver<TSource> : IChangeStreamObserver<TSource>
    where TSource : ICollectionMarker
{
    private readonly ILogger<ChangeStreamObserver<TSource>> _logger;

    private readonly IChangeStreamProcessor<TSource> _processor;
    private readonly IChangeStreamTokenRepository _changeStreamTokenRepository;

    public ChangeStreamObserver(
        ILogger<ChangeStreamObserver<TSource>> logger, 
        IChangeStreamTokenRepository changeStreamTokenRepository, 
        IChangeStreamProcessor<TSource> processor)
    {
        _logger = logger;
        _changeStreamTokenRepository = changeStreamTokenRepository;
        _processor = processor;
    }

    public async Task ObserveCollection(string connectionString, string databaseName, CancellationToken cancellationToken = default)
    {
        // TODO: Check if creating mongo client like this is efficient
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);

        await ObserveCollection(database, cancellationToken);
    }

    public async Task ObserveCollection(IMongoDatabase database, CancellationToken cancellationToken = default)
    {
        var collection = database.GetCollection<TSource>(MongoCollectionMarkerValidator.ThrowErrorOrCollectionNameAttributeValue<TSource>().CollectionName);

        var userChangeStreamWatchKey = _processor.GenerateKey();

        BsonDocument? resumeToken = await _changeStreamTokenRepository.GetToken(userChangeStreamWatchKey);
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup,
            BatchSize = 1,
            StartAfter = resumeToken
        };

        string filter = _processor.GenerateFilter();

        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<TSource>>().Match(filter);

        using (var cursor = await collection.WatchAsync(pipeline, options, cancellationToken))
        {
            while (await cursor.MoveNextAsync(cancellationToken))
            {
                foreach (var currentDocument in cursor.Current)
                {
                    try
                    {
                        await _processor.InitiateProcessing(currentDocument);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in business workflow", ex);
                    }

                    try
                    {
                        await _changeStreamTokenRepository.SaveToken(userChangeStreamWatchKey, currentDocument.DocumentKey, currentDocument.ResumeToken, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Failed to update resume token", ex);
                    }
                }
            }
        }
    }
}
