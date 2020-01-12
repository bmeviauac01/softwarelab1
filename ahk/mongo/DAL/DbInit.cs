using ahk.common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace adatvez.DAL
{
    public static class DbInit
    {
        public static void InitializeDatabase(AhkResult result)
        {
            waitForServer();

            var createDbScript = getDatabaseInitScript();
            createAndInitDatabase(createDbScript);
            validateDatabase();
        }

        private static void waitForServer()
        {
            Console.WriteLine("Adatbazis szerver kapcsolodas folyamatban...");

            var client = DbFactory.Client;
            var database = client.GetDatabase("admin");
            var collection = database.GetCollection<BsonDocument>("system.version");

            var waitUntil = DateTime.UtcNow.AddMinutes(1);
            while (true)
            {
                try
                {
                    var version = collection
                        .Find(d => d["_id"] == "featureCompatibilityVersion")
                        .Single();

                    Console.WriteLine($"Adatbazis szerver (MongoDB {version["version"].AsString}) kapcsolodas sikeres");
                    return;
                }
                catch
                {
                    System.Threading.Thread.Sleep(5);

                    if (DateTime.UtcNow > waitUntil)
                    {
                        Console.WriteLine("Adatbazis szerver kapcsolodas SIKERTELEN; ez valoszinuleg nem a megoldas hibaja");
                        throw;
                    }
                }
            }
        }

        private static string getDatabaseInitScript()
        {
            string createDbScript;
            using (var client = new HttpClient())
                createDbScript = client.GetStringAsync("https://raw.githubusercontent.com/bmeviauac01/gyakorlatok/master/mongo.js").Result;
            Console.WriteLine("Adatbazis init script letoltve");                

            return createDbScript;
        }

        private static void createAndInitDatabase(string createDbScript)
        {
            try
            {
                var database = DbFactory.Database;
                var regex = new Regex(@"db\.(?<collectionName>\w*?)\.insertMany\(\[(?<documents>.*?)\]\)", RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                foreach (Match match in regex.Matches(createDbScript))
                {
                    var collectionName = match.Groups["collectionName"].Value;
                    var collection = database.GetCollection<BsonDocument>(collectionName);

                    var documents = BsonSerializer.Deserialize<BsonDocument[]>('[' + match.Groups["documents"].Value + ']');
                    collection.InsertMany(documents);
                }

                Console.WriteLine("Adatbazis init script lefuttatva");
            }
            catch (Exception ex)
            {
                throw new Exception("Az adatbazis letrehozasa nem sikerult", ex);
            }
        }

        private static void validateDatabase()
        {
            try
            {
                var termekCollection = DbFactory.TermekCollection;
                if (termekCollection.CountDocuments(_ => true) == 0)
                {
                    throw new Exception("Az adatbazis ures?!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Adatbazis nem erheto el", ex);
            }
        }
    }
}
