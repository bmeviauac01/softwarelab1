using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace adatvez.DAL.Entities
{   
    public class SzamlaKiallito
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Nev { get; set; }
        public string IR { get; set; }
        public string Varos { get; set; }
        public string Utca { get; set; }
        public string Adoszam { get; set; }
        public string Szamlaszam { get; set; }
    }
}
