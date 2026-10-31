using MongoDB.Driver;

namespace Hotel.Nosql;

public static class MongoDbFactory
{
    public static IMongoDatabase Create(string connectionString = "mongodb://localhost:27017", string databaseName = "HotelDb")
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3);

        var client = new MongoClient(settings);
        return client.GetDatabase(databaseName);
    }
}