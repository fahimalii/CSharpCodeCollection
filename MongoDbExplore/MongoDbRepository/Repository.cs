using MongoDB.Driver;

namespace MongoDbRepository;

public interface IRepository<T> where T : class
{
    Task Insert(T entity, CancellationToken cancellationToken = default);
    Task<IList<T>> Get(FilterDefinition<T> filter);
    Task<ReplaceOneResult> Upsert(T entity, FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task InsertMany(IReadOnlyCollection<T> entities, CancellationToken cancellationToken = default);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<T> _mongoCollection;

    public Repository(IMongoClient mongoClient, string databaseName)
    {
        _mongoClient = mongoClient;
        _mongoDatabase = _mongoClient.GetDatabase(databaseName);

        var collectionNameInfo = MongoCollectionMarkerValidator.ThrowErrorOrCollectionNameAttributeValue<T>();

        _mongoCollection = _mongoDatabase.GetCollection<T>(collectionNameInfo.CollectionName);
    }

    public async Task Insert(T entity, CancellationToken cancellationToken = default)
    {
        await _mongoCollection.InsertOneAsync(entity, new InsertOneOptions 
        { 
            BypassDocumentValidation = false, 
            Comment = "Test Comment"
        }, cancellationToken);
    }

    public async Task InsertMany(IReadOnlyCollection<T> entities, CancellationToken cancellationToken = default)
    {
        await _mongoCollection.InsertManyAsync(entities, new InsertManyOptions
        {
            BypassDocumentValidation = false,
            Comment = "Test Comment",
            IsOrdered = true
        }, cancellationToken);
    }

    public async Task<IList<T>> Get(FilterDefinition<T> filter)
    {
        var data = await _mongoCollection.FindAsync(filter);
        var result = data.ToList();

        return result;
    }

    public async Task<ReplaceOneResult> Upsert(T entity, FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        ReplaceOneResult upsertOperationResult = await _mongoCollection
            .ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);

        return upsertOperationResult;
    }
}
