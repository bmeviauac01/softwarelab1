using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace adatvez.DAL.Entities
{
    public class Kategoria
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Nev { get; set; }
        public ObjectId? SzuloKategoriaID { get; set; }
    }
}
