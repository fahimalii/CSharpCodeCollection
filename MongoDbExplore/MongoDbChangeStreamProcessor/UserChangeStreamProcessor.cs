using MongoDB.Driver;
using MongoDbRepository.EntityModel;

namespace MongoDbChangeStreamProcessor;
public class UserChangeStreamProcessor : IChangeStreamProcessor<User>
{
    public string GenerateFilter()
    {
        return "{ operationType: { $in: ['insert', 'update', 'delete'] } }";
    }

    public string GenerateKey()
    {
        return "token_user";
    }

    public Task InitiateProcessing(ChangeStreamDocument<User> document)
    {
        Console.WriteLine($"Processing {document.FullDocument.Email}");

        return Task.CompletedTask;
    }
}
