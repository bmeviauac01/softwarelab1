using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace adatvez.DAL.Entities
{ 
    public class Megrendeles
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId VevoID { get; set; }
        public ObjectId TelephelyID { get; set; }

        public DateTime? Datum { get; set; }
        public DateTime? Hatarido { get; set; } 
        public string Statusz { get; set; }
        public FizetesMod FizetesMod { get; set; }
        public MegrendelesTetel[] MegrendelesTetelek { get; set; }
        public Szamla Szamla { get; set; }
    }
}
