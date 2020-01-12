using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace adatvez.DAL
{
    public static class DbFactory
    {
        private static readonly Lazy<IMongoClient> mongoClient = new Lazy<IMongoClient>(() =>
        {
#if DEBUG
            var connectionString = "mongodb://192.168.198.131:27017/aaf";
#else
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTIONSTRING");
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Nincs konfiguralva az adatbazis eleres.");
#endif

            var pack = new ConventionPack
            {
                new AAFElementNameConvention(),
            };
            ConventionRegistry.Register("AAFConventions", pack, _ => true);

            return new MongoClient(connectionString);

        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static IMongoClient Client => mongoClient.Value;
        public static IMongoDatabase Database => Client.GetDatabase("aaf");

        public static IMongoCollection<Entities.Termek> TermekCollection => Database.GetCollection<Entities.Termek>("termekek");
        public static IMongoCollection<Entities.Kategoria> KategoriaCollection => Database.GetCollection<Entities.Kategoria>("kategoriak");
        public static IMongoCollection<Entities.Vevo> VevoCollection => Database.GetCollection<Entities.Vevo>("vevok");
        public static IMongoCollection<Entities.Megrendeles> MegrendelesCollection => Database.GetCollection<Entities.Megrendeles>("megrendelesek");
    }
}
