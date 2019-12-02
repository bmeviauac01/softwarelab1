using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class SzamlaKiallito
    {
        public SzamlaKiallito()
        {
            Szamla = new HashSet<Szamla>();
        }

        public int Id { get; set; }
        public string Nev { get; set; }
        public string Ir { get; set; }
        public string Varos { get; set; }
        public string Utca { get; set; }
        public string Adoszam { get; set; }
        public string Szamlaszam { get; set; }

        public ICollection<Szamla> Szamla { get; set; }
    }
}
