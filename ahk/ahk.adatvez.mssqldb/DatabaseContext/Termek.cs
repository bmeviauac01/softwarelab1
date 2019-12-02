using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class Termek
    {
        public Termek()
        {
            MegrendelesTetel = new HashSet<MegrendelesTetel>();
        }

        public int Id { get; set; }
        public string Nev { get; set; }
        public double? NettoAr { get; set; }
        public int? Raktarkeszlet { get; set; }
        public int? Afaid { get; set; }
        public int? KategoriaId { get; set; }
        public string Leiras { get; set; }
        public byte[] Kep { get; set; }

        public Afa Afa { get; set; }
        public Kategoria Kategoria { get; set; }
        public ICollection<MegrendelesTetel> MegrendelesTetel { get; set; }
    }
}
