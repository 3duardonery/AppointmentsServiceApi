using AppointmentService.Shared.Settings;
using MongoDB.Driver;

namespace AppointmentService.Data.DataContext
{
    public sealed class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IMongoClient client, AppSettings appSettings) 
            => _database = client.GetDatabase(appSettings?.Database);

        public IMongoCollection<T> GetCollection<T>(string name) 
            => _database.GetCollection<T>(name);
    }
}
