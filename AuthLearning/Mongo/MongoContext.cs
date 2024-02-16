using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthLearning.Mongo
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext()
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var databaseName = configuration.GetSection("DatabaseName").Value;
                var connectionString = configuration.GetConnectionString("LocalUrl");
                var mongoClient = new MongoClient(connectionString);
                _database = mongoClient.GetDatabase(databaseName);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            CreateCollectionIfNotExists(collectionName); 
            return _database.GetCollection<T>(collectionName);
        }

        public void CreateCollectionIfNotExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = _database.ListCollections(new ListCollectionsOptions { Filter = filter });
            if (!collections.Any())
            {
                _database.CreateCollection(collectionName);
            }
        }
    }
}
