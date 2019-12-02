using System;
using System.Collections.Generic;

namespace ahk.adatvez.mssqldb.DatabaseContext
{
    public partial class Telephely
    {
        public Telephely()
        {
            Megrendeles = new HashSet<Megrendeles>();
            VevoNavigation = new HashSet<Vevo>();
        }

        public int Id { get; set; }
        public string Ir { get; set; }
        public string Varos { get; set; }
        public string Utca { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public int? VevoId { get; set; }

        public Vevo Vevo { get; set; }
        public ICollection<Megrendeles> Megrendeles { get; set; }
        public ICollection<Vevo> VevoNavigation { get; set; }
    }
}
