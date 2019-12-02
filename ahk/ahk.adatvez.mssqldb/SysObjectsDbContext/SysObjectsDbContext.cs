using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ahk.adatvez.mssqldb.SysObjectsDbContext
{
    public partial class SysObjectsDbContext : DbContext
    {
        public SysObjectsDbContext()
        {
        }

        public SysObjectsDbContext(DbContextOptions<SysObjectsDbContext> options)
            : base(options)
        {
        }

        internal virtual DbSet<SysObject> SysObjects { get; set; }

        public IEnumerable<SysObject> ListSysObjects()
            => SysObjects.FromSql("select object_id, name, type from sys.objects");

        internal virtual DbSet<SysColumn> SysColumns { get; set; }

        public IEnumerable<SysColumn> ListSysColumns()
            => SysColumns.FromSql("select table_name, column_name from information_schema.columns");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SysColumn>().HasKey(sc => new { sc.TableName, sc.ColumnName });
        }
    }
}
