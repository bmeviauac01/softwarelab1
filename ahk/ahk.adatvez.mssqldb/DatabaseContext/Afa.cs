using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class Afa
    {
        public Afa()
        {
            Termek = new HashSet<Termek>();
        }

        public int Id { get; set; }
        public int? Kulcs { get; set; }

        public ICollection<Termek> Termek { get; set; }
    }
}
