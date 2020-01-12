using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace adatvez.DAL.Entities
{
    public class Telephely
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string IR { get; set; }
        public string Varos { get; set; }
        public string Utca { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
    }
}
