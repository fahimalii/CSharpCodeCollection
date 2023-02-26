using MongoDB.Driver;
using MongoDbRepository.EntityModel;

namespace MongoDbRepository;

public class MongoCollectionProvider
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoCollectionProvider(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public IMongoCollection<User> UserCollection => GetCollection<User>();
    public IMongoCollection<ChangeStreamResumeToken> ChangeStreamResumeTokenCollection => GetCollection<ChangeStreamResumeToken>();

    public IMongoCollection<T> GetCollection<T>()
    {
        var collectionNameAttribute = MongoCollectionMarkerValidator.ThrowErrorOrCollectionNameAttributeValue<T>();

        return _mongoDatabase.GetCollection<T>(collectionNameAttribute.CollectionName);
    }
}
