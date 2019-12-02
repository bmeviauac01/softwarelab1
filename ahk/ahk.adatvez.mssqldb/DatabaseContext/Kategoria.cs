using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class Kategoria
    {
        public Kategoria()
        {
            InverseSzuloKategoriaNavigation = new HashSet<Kategoria>();
            Termek = new HashSet<Termek>();
        }

        public int Id { get; set; }
        public string Nev { get; set; }
        public int? SzuloKategoria { get; set; }

        public Kategoria SzuloKategoriaNavigation { get; set; }
        public ICollection<Kategoria> InverseSzuloKategoriaNavigation { get; set; }
        public ICollection<Termek> Termek { get; set; }
    }
}
