using MongoDB.Driver;
using Webscraper_API.Scraper.TCG_Magic.Model;

namespace Web_API.Data
{
    public class MagicDbContext
    {
        public IMongoCollection<Card> Cards { get; set; }

        public MagicDbContext(IServiceProvider service)
        {
            var config = service.GetRequiredService<IConfiguration>();
            var client = service.GetRequiredService<MongoClient>();

            var db = client.GetDatabase(config["MongoDb:DatabaseNames:Magic"]);

            Cards = db.GetCollection<Card>(config["MongoDb:CollectionNames:Cards"]);
        }
    }
}
