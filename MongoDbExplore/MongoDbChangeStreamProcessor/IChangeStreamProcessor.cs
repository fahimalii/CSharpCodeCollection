using MongoDB.Driver;

namespace MongoDbChangeStreamProcessor;

/// <summary>
/// Contract implemented by each processor which processes the changes in a collection
/// </summary>
/// <typeparam name="TSource">The model which represents the collection being observed</typeparam>
public interface IChangeStreamProcessor<TSource>
{
    /// <summary>
    /// The processing work done based on the document that has changed. Can be some business workflow
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    Task InitiateProcessing(ChangeStreamDocument<TSource> document);

    /// <summary>
    /// Unique key used for observing this collection. This is stored in resume token to continue watch later if watch processor is not running
    /// </summary>
    /// <returns></returns>
    string GenerateKey();

    /// <summary>
    /// The filters for the change stream observer. What operations should be observed for the collection
    /// </summary>
    /// <returns></returns>
    string GenerateFilter();
}