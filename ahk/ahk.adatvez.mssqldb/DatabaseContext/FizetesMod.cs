using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class FizetesMod
    {
        public FizetesMod()
        {
            Megrendeles = new HashSet<Megrendeles>();
        }

        public int Id { get; set; }
        public string Mod { get; set; }
        public int? Hatarido { get; set; }

        public ICollection<Megrendeles> Megrendeles { get; set; }
    }
}
