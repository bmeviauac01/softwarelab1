using System;
using System.Collections.Generic;

namespace adatvez.DatabaseContext
{
    public partial class Megrendeles
    {
        public Megrendeles()
        {
            MegrendelesTetel = new HashSet<MegrendelesTetel>();
            Szamla = new HashSet<Szamla>();
        }

        public int Id { get; set; }
        public DateTime? Datum { get; set; }
        public DateTime? Hatarido { get; set; }
        public int? TelephelyId { get; set; }
        public int? StatuszId { get; set; }
        public int? FizetesModId { get; set; }

        public FizetesMod FizetesMod { get; set; }
        public Statusz Statusz { get; set; }
        public Telephely Telephely { get; set; }
        public ICollection<MegrendelesTetel> MegrendelesTetel { get; set; }
        public ICollection<Szamla> Szamla { get; set; }
    }
}
