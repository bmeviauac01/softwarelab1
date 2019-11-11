using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace adatvez.SysObjectsDbContext
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

        public virtual DbSet<SysObject> SysObjects { get; set; }

        public IEnumerable<SysObject> ListSysObjects()
            => SysObjects.FromSql("select object_id, name, type from sys.objects");
    }
}
