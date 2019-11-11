using System.ComponentModel.DataAnnotations;

namespace adatvez.KategoriaSzulovelDatabaseContext
{
    public partial class KategoriaSzulovel
    {
        [Key]
        public string KategoriaNev { get; set; }
        public string SzuloKategoriaNev { get; set; }
    }
}
