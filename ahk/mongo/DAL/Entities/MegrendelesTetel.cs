using MongoDB.Bson;

namespace adatvez.DAL.Entities
{   
    public class MegrendelesTetel
    {
        public int? Mennyiseg { get; set; }
        public double? NettoAr { get; set; }
        public ObjectId TermekID { get; set; }
        public string Statusz { get; set; }
        public SzamlaTetel SzamlaTetel { get; set; }
    }
}
