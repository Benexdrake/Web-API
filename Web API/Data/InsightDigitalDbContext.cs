using MongoDB.Driver;
using Webscraper_API.Scraper.Insight_Digital_Handy.Models;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Data
{
    public class InsightDigitalDbContext
    {
        public IMongoCollection<Handy> Handys { get; set; }

        public InsightDigitalDbContext(IServiceProvider service)
        {
            var config = service.GetRequiredService<IConfiguration>();
            var client = service.GetRequiredService<MongoClient>();

            var db = client.GetDatabase(config["MongoDb:DatabaseNames:InsightDigital"]);

            Handys = db.GetCollection<Handy>(config["MongoDb:CollectionNames:InsightDigitalHandys"]);
        }
    }
}
