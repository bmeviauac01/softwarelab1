using System;
using System.Collections.Generic;

namespace adatvez.DatabaseContext
{
    public partial class Statusz
    {
        public Statusz()
        {
            Megrendeles = new HashSet<Megrendeles>();
            MegrendelesTetel = new HashSet<MegrendelesTetel>();
        }

        public int Id { get; set; }
        public string Nev { get; set; }

        public ICollection<Megrendeles> Megrendeles { get; set; }
        public ICollection<MegrendelesTetel> MegrendelesTetel { get; set; }
    }
}
