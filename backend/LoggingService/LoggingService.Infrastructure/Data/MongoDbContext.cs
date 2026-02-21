using LoggingService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace LoggingService.Infrastructure.Data
{
    // MongoDB database context
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        // Collections
        public IMongoCollection<RequestLog> RequestLogs => 
            _database.GetCollection<RequestLog>("RequestLogs");

        public IMongoCollection<ErrorLog> ErrorLogs => 
            _database.GetCollection<ErrorLog>("ErrorLogs");
    }
}