using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbRepository.Constants;
using MongoDbRepository.Attributes;

namespace MongoDbRepository.EntityModel;

[CollectionName(MongoDbCollectionNames.ChangeStreamTokenCollection)]
public sealed class ChangeStreamResumeToken
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; }

    public BsonDocument Token { get; set; }

    public BsonDocument DocumentKey { get; set; }

    public DateTime LastModified
    {
        get => _lastModified;
        private set => _lastModified = value;
    }

    private DateTime _lastModified = DateTime.UtcNow;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
