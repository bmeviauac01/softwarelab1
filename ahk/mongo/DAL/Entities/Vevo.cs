using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace adatvez.DAL.Entities
{
    public class Vevo
    {
        [BsonId]
        public ObjectId ID { get; set; }

        public string Nev { get; set; }
        public string Szamlaszam { get; set; }
        public string Login { get; set; }
        public string Jelszo { get; set; }
        public string Email { get; set; }

        public ObjectId KozpontiTelephelyID { get; set; }
        public Telephely[] Telephelyek { get; set; }
    }
}
