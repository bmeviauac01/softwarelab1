using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoLabor.DAL.Entities;
using System;

namespace adatvez.DAL
{
    public static class DbFactory
    {
        public const string DatabaseName = "aaf"; 

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
        public static IMongoDatabase Database => Client.GetDatabase(DatabaseName);

        public static IMongoCollection<Termek> TermekCollection => Database.GetCollection<Termek>("termekek");
        public static IMongoCollection<Kategoria> KategoriaCollection => Database.GetCollection<Kategoria>("kategoriak");
        public static IMongoCollection<Vevo> VevoCollection => Database.GetCollection<Vevo>("vevok");
        public static IMongoCollection<Megrendeles> MegrendelesCollection => Database.GetCollection<Megrendeles>("megrendelesek");
    }
}
