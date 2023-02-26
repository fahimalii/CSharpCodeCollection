// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDbRepository;

using MongoDbRepository.EntityModel;
using MongoDbSampleDataGenerator;


IReadOnlyList<User> users = UserGenerator.Generate();

// TODO ADD YOUR DB CONNECTION HERE
const string connectionString = "mongodb://192.168.0.240:27021,192.168.0.240:27022,192.168.0.240:27023/?replicaSet=rs0&authSource=admin";
const string databaseName = "development";

if(string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"{nameof(connectionString)} and {nameof(databaseName)} required");
    Console.ResetColor();
    return;
}

var categoriesConfiguration = new Dictionary<string, string?>
{
    { "LogLevel:Default", "Error" },
    { "LogLevel:MongoDB.SDAM", "Debug" },
    { "LogLevel:MongoDB.ServerSelection", "Debug" },
    { "LogLevel:MongoDB.Command", "Debug" },
    { "LogLevel:MongoDB.Connection", "Debug" }
};

var config = new ConfigurationBuilder()
  .AddInMemoryCollection(categoriesConfiguration)
  .Build();

using var loggerFactory = LoggerFactory.Create(b =>
{
    b.AddConfiguration(config);
    b.AddSimpleConsole();
});

var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);

mongoClientSettings.LoggingSettings = new LoggingSettings(loggerFactory);

var mongoClient = new MongoClient(mongoClientSettings);
var repository = new Repository<User>(mongoClient, databaseName);
var data = await repository.Get(Builders<User>.Filter.Empty);

await repository.InsertMany(users);