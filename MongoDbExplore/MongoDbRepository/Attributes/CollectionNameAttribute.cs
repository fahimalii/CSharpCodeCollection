namespace MongoDbRepository.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class CollectionNameAttribute : Attribute
{
    public CollectionNameAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }

    public string CollectionName { get; init; }
}
