using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbChangeStreamProcessor;
using MongoDbRepository;
using MongoDbRepository.EntityModel;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddOptions<MongoDbConfiguration>()
            .BindConfiguration("MongoDb")
            .Validate(x =>
            {
                if (string.IsNullOrWhiteSpace(x.ConnectionString) || string.IsNullOrWhiteSpace(x.DatabaseName))
                    return false;

                return true;
            })
            .ValidateOnStart();

        services.AddSingleton<IMongoClient>(x =>
        {
            var mongoDbOptions = x.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;
            var mongoClient = new MongoClient(mongoDbOptions.ConnectionString);
            var database = mongoClient.GetDatabase(mongoDbOptions.DatabaseName);
            bool isMongoLive = MongoDbLiveValidator.IsLive(database);

            if (isMongoLive == false)
                throw new Exception("Database connection failed");
            
            return mongoClient;
        });

        services.AddScoped<IChangeStreamTokenRepository, ChangeStreamTokenRepository>();
        services.AddScoped<IRepository<ChangeStreamResumeToken>>(x =>
        {
            var mongoDbOptions = x.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;
            var mongoClient = x.GetRequiredService<IMongoClient>();
            
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(mongoDbOptions.DatabaseName));

            return new Repository<ChangeStreamResumeToken>(mongoClient, mongoDbOptions.DatabaseName!);
        });

        services.AddScoped<IChangeStreamObserver<User>, ChangeStreamObserver<User>>();
        services.AddScoped<IChangeStreamProcessor<User>, UserChangeStreamProcessor>();

        services.AddHostedService<UserChangeStreamProcessorWorker>();
    })
    .Build();

host.Run();
