using System;

namespace adatvez.DAL.Entities
{
    public class Szamla
    {
        public string MegrendeloNev { get; set; }
        public string MegrendeloIR { get; set; }
        public string MegrendeloVaros { get; set; }
        public string MegrendeloUtca { get; set; }
        public int? NyomtatottPeldanyszam { get; set; }
        public bool? Sztorno { get; set; }
        public string FizetesiMod { get; set; }
        public DateTime? KiallitasDatum { get; set; }
        public DateTime? TeljesitesDatum { get; set; }
        public DateTime? FizetesiHatarido { get; set; }
        public SzamlaKiallito Kiallito { get; set; }
    }
}
