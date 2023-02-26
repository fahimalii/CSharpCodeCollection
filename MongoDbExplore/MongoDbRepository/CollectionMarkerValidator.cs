using System.ComponentModel;
using System.Reflection;
using MongoDbRepository.Attributes;

namespace MongoDbRepository;

public static class MongoCollectionMarkerValidator
{
    /// <summary>
    /// Method which will validate that if a class implements <see cref="ICollectionMarker"/>
    /// then it should have the attribute <see cref="CollectionNameAttribute"/> to point to the mongo collection it represents.
    /// This can be used during startup of application.
    /// Obviously it depends on the use case of marking all models that map to some mongo collection directly with the <see cref="ICollectionMarker"/>
    /// </summary>
    public static void ValidateCollectionMarker()
    {
        // we filter the defined classes according to the interfaces they implement
        var mongoCollectionModels = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ICollectionMarker).IsAssignableFrom(type) && !type.IsInterface)
                .ToList();

        foreach (var mongoModel in mongoCollectionModels)
        {
            ThrowErrorOrCollectionNameAttributeValue(mongoModel);
        }
    }

    /// <summary>
    /// Returns collection info with which this entity is bound or throws exception
    /// </summary>
    /// <param name="type">Enity type</param>
    /// <exception cref="ArgumentNullException">This exception is thrown if attribute does not exist on type</exception>
    /// <returns><see cref="CollectionNameAttribute"/></returns>
    public static CollectionNameAttribute ThrowErrorOrCollectionNameAttributeValue(Type type)
    {
        var collectionNameAttribute = GetCollectionNameAttribute(type);

        if (collectionNameAttribute == null)
            throw new ArgumentNullException(nameof(CollectionNameAttribute),
                                            $"Required attribute missing on type {type.FullName}");

        return collectionNameAttribute;
    }

    /// <summary>
    /// Returns collection info with which this entity is bound or throws exception
    /// </summary>
    /// <param name="type">Enity type</param>
    /// <exception cref="ArgumentNullException">This exception is thrown if attribute does not exist on type</exception>
    /// <returns><see cref="CollectionNameAttribute"/></returns>
    public static CollectionNameAttribute ThrowErrorOrCollectionNameAttributeValue<T>() => ThrowErrorOrCollectionNameAttributeValue(typeof(T));

    private static CollectionNameAttribute? GetCollectionNameAttribute(Type type) =>
        TypeDescriptor.GetAttributes(type).OfType<CollectionNameAttribute>().FirstOrDefault();
}
