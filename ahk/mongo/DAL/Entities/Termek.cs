using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace adatvez.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class Termek
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId KategoriaID { get; set; }

        public string Nev { get; set; }
        public double? NettoAr { get; set; }
        public int? Raktarkeszlet { get; set; }
        public AFA AFA { get; set; }
    }
}
