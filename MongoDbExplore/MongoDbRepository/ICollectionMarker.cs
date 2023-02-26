using MongoDbRepository.Attributes;

namespace MongoDbRepository;

/// <summary>
/// Marker interface added in all models which map to Mongo Collection
/// Used during startup to ensure all models have <see cref="CollectionNameAttribute"/> if they map directly to some db collection and used to do db operation
/// </summary>
public interface ICollectionMarker
{
}
