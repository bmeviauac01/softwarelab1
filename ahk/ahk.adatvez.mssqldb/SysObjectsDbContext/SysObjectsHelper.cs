using System;
using System.Linq;
using ahk.common;

namespace ahk.adatvez.mssqldb
{
    public static class SysObjectsHelper
    {
        public static bool TriggerExistsWithName(string triggerName, ref AhkResult result)
            => SysObjectExistsWithName(triggerName, "TR", "trigger", ref result);

        public static bool StoredProcedureExistsWithName(string precedureName, ref AhkResult result)
            => SysObjectExistsWithName(precedureName, "P", "tarolt eljaras", ref result);

        private static bool SysObjectExistsWithName(string objectName, string objectTypeIdInMssql, string objectTypeDescription, ref AhkResult result)
        {
            var sysobjectsDbContextOptions = DbFactory.GetDbOptions<SysObjectsDbContext.SysObjectsDbContext>();
            using (var db = new SysObjectsDbContext.SysObjectsDbContext(sysobjectsDbContextOptions))
            {
                var item = db.ListSysObjects().FirstOrDefault(obj => obj.Type.Trim().Equals(objectTypeIdInMssql, StringComparison.OrdinalIgnoreCase) && obj.Name.Trim().Equals(objectName, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                {
                    result.AddProblem($"Nem letezik a {objectName} {objectTypeDescription}");
                    return false;
                }
                else
                {
                    result.Log($"{objectName} {objectTypeDescription} letezik");
                    return true;
                }
            }
        }

        public static bool ColumnExistsInTable(string tableName, string columnName, ref AhkResult result)
        {
            var sysobjectsDbContextOptions = DbFactory.GetDbOptions<SysObjectsDbContext.SysObjectsDbContext>();
            using (var db = new SysObjectsDbContext.SysObjectsDbContext(sysobjectsDbContextOptions))
            {
                var item = db.ListSysColumns().FirstOrDefault(obj => obj.TableName.Trim().Equals(tableName, StringComparison.OrdinalIgnoreCase) && obj.ColumnName.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                {
                    result.AddProblem($"Nem letezik a {tableName}.{columnName} oszlop");
                    return false;
                }
                else
                {
                    result.Log($"{tableName}.{columnName} letezik");
                    return true;
                }
            }
        }
    }
}
