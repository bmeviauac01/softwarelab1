using System.ComponentModel.DataAnnotations.Schema;

namespace adatvez.SysObjectsDbContext
{
    public partial class SysColumn
    {
        [Column("table_name")]
        public string TableName { get; set; }

        [Column("column_name")]
        public string ColumnName { get; set; }
    }
}
