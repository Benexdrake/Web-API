using MongoDB.Driver;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Data
{
    public class SteamDbContext
    {
        public IMongoCollection<Game> Games { get; set; }

        public SteamDbContext(IServiceProvider service)
        {
            var config = service.GetRequiredService<IConfiguration>();
            var client = service.GetRequiredService<MongoClient>();

            var db = client.GetDatabase(config["MongoDb:DatabaseNames:Steam"]);

            Games = db.GetCollection<Game>(config["MongoDb:CollectionNames:Steam"]);
        }
    }
}
