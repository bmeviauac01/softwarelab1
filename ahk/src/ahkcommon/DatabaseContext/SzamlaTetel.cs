using System;
using System.Collections.Generic;

namespace adatvez.DatabaseContext
{
    public partial class SzamlaTetel
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public int? Mennyiseg { get; set; }
        public double? NettoAr { get; set; }
        public int? Afakulcs { get; set; }
        public int? SzamlaId { get; set; }
        public int? MegrendelesTetelId { get; set; }

        public MegrendelesTetel MegrendelesTetel { get; set; }
        public Szamla Szamla { get; set; }
    }
}
