using Microsoft.EntityFrameworkCore;

namespace adatvez.KategoriaSzulovelDatabaseContext
{
    public partial class KategoriaSzulovelDatabaseContext : DbContext
    {
        public KategoriaSzulovelDatabaseContext()
        {
        }

        public KategoriaSzulovelDatabaseContext(DbContextOptions<KategoriaSzulovelDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<KategoriaSzulovel> KategoriaSzulovel { get; set; }
    }
}
