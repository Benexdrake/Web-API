using MongoDB.Driver;
using Webscraper_API.Scraper.Dota2.Models;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Data
{
    public class Dota2DbContext
    {
        public IMongoCollection<Hero> Heroes { get; set; }

        public Dota2DbContext(IServiceProvider service)
        {
            var config = service.GetRequiredService<IConfiguration>();
            var client = service.GetRequiredService<MongoClient>();

            var db = client.GetDatabase(config["MongoDb:DatabaseNames:Dota2"]);

            Heroes = db.GetCollection<Hero>(config["MongoDb:CollectionNames:Heroes"]);
        }
    }
}
