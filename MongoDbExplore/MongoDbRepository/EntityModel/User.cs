using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbRepository.Constants;
using MongoDbRepository.Attributes;

namespace MongoDbRepository.EntityModel;

[CollectionName(MongoDbCollectionNames.UserCollection)]
public sealed class User : ICollectionMarker
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public DateTime LastLoggedIn { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}