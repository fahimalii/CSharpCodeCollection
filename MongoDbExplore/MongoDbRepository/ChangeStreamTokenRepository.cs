using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbRepository.EntityModel;

namespace MongoDbRepository;

public interface IChangeStreamTokenRepository
{
    Task<BsonDocument?> GetToken(string tokenKey);
    Task<ReplaceOneResult?> SaveToken(string tokenKey, BsonDocument document, BsonDocument token, CancellationToken cancellationToken = default);
}

public class ChangeStreamTokenRepository : IChangeStreamTokenRepository
{
    private readonly IRepository<ChangeStreamResumeToken> _resumeTokenRepository;

    public ChangeStreamTokenRepository(IRepository<ChangeStreamResumeToken> resumeTokenRepository)
    {
        _resumeTokenRepository = resumeTokenRepository;
    }

    public async Task<BsonDocument?> GetToken(string tokenKey)
    {
        var filter = Builders<ChangeStreamResumeToken>.Filter.Eq(x => x.Id, tokenKey);
        var resumeToken = (await _resumeTokenRepository.Get(filter)).FirstOrDefault();

        return resumeToken?.Token;
    }

    public async Task<ReplaceOneResult?> SaveToken(string tokenKey, BsonDocument document, BsonDocument token, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChangeStreamResumeToken>.Filter.Eq(x => x.Id, tokenKey);

        var upsertOperationResult = await _resumeTokenRepository.Upsert(new ChangeStreamResumeToken
        {
            Id = tokenKey,
            DocumentKey = document,
            Token = token
        }, filter, cancellationToken);

        return upsertOperationResult;
    }
}
