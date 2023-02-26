using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbRepository;
public static class MongoDbLiveValidator
{
    /// <summary>
    /// Checks if database connection is working with ping
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    public static bool IsLive(IMongoDatabase database, int milisecondTimeout = 5000)
    {
        bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(milisecondTimeout);
        return isMongoLive;
    }
}
