using MongoDB.Driver;

namespace Web_API.Data
{
    public class CrunchyrollDBContext
    {
        public IMongoCollection<Anime> Animes { get; set; }
        public IMongoCollection<Episode> Episodes { get; set; }

        public CrunchyrollDBContext(IServiceProvider service)
        {
            var config = service.GetRequiredService<IConfiguration>();
            var client = service.GetRequiredService<MongoClient>();

            var db = client.GetDatabase(config["MongoDb:DatabaseNames:Crunchyroll"]);

            Animes = db.GetCollection<Anime>(config["MongoDb:CollectionNames:CrunchyrollAnimes"]);
            Episodes = db.GetCollection<Episode>(config["MongoDb:CollectionNames:CrunchyrollEpisodes"]);
        }
    }
}
