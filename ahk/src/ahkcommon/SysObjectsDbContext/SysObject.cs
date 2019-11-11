using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace adatvez.SysObjectsDbContext
{
    public partial class SysObject
    {
        [Key]
        [Column("object_id")]
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
