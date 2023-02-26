using Bogus;
using MongoDB.Bson;
using MongoDbRepository.Constants;
using MongoDbRepository.EntityModel;
using System.Text.Json;

namespace MongoDbSampleDataGenerator;

public class UserGenerator
{
    private static readonly DateTime _start = new DateTime(2021, 01, 01);
    private static readonly DateTime _end = new DateTime(2023, 02, 26);

    private static readonly Faker<User> fakeUserGenerator = new Faker<User>()
            .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(u => u.Username, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(GetGenderEnumFromString(u.Gender)))
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(GetGenderEnumFromString(u.Gender)))
            .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
            .RuleFor(u => u.Gender, f => GetGenderStringFromEnum(f.PickRandom<Bogus.DataSets.Name.Gender>()))
            .RuleFor(u => u.LastLoggedIn, f => f.Date.Between(_start, _end));

    public static IReadOnlyList<User> Generate(uint count = 10, bool printJson = false)
    {
        IList<User> users = new List<User>();

        for (int i = 0; i < count; i++)
        {
            var temp = fakeUserGenerator.Generate();

            if (printJson)
                PrintAsJson(temp);

            users.Add(temp);
        }

        return users.AsReadOnly();
    }

    static void PrintAsJson<T>(T data)
    {
        Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    static Bogus.DataSets.Name.Gender GetGenderEnumFromString(string name) => name switch
    {
        GenderConstants.Male => Bogus.DataSets.Name.Gender.Male,
        _ => Bogus.DataSets.Name.Gender.Female
    };

    static string GetGenderStringFromEnum(Bogus.DataSets.Name.Gender gender) => gender switch
    {
        Bogus.DataSets.Name.Gender.Male => GenderConstants.Male,
        _ => GenderConstants.Female
    };
}
