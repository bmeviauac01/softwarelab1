using ahk.common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ahk.adatvez.mssqldb
{
    public static class SysObjectsHelper
    {
        public static bool TriggerExistsWithName(string triggerName, AhkResult result)
            => SysObjectExistsWithName(triggerName, "TR", "trigger", result);

        public static bool StoredProcedureExistsWithName(string precedureName, AhkResult result)
            => SysObjectExistsWithName(precedureName, "P", "tarolt eljaras", result);

        private static bool SysObjectExistsWithName(string objectName, string objectTypeIdInMssql, string objectTypeDescription, AhkResult result)
        {
            var item = ListSysObjects().FirstOrDefault(obj => obj.Type.Trim().Equals(objectTypeIdInMssql, StringComparison.OrdinalIgnoreCase) && obj.Name.Trim().Equals(objectName, StringComparison.OrdinalIgnoreCase));
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

        public static bool ColumnExistsInTable(string tableName, string columnName, AhkResult result)
        {
            var item = ListSysColumns().FirstOrDefault(obj => obj.TableName.Trim().Equals(tableName, StringComparison.OrdinalIgnoreCase) && obj.ColumnName.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase));
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

        public static IReadOnlyList<SysObject> ListSysObjects()
        {
            var result = new List<SysObject>();
            using (var conn = new SqlConnection(DbFactory.GetConnectionString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select object_id, name, type from sys.objects";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                            result.Add(new SysObject() { ObjectId = rdr.GetInt32(0), Name = rdr.GetString(1), Type = rdr.GetString(2) });
                    }
                }
            }
            return result;
        }

        public static IEnumerable<SysColumn> ListSysColumns()
        {
            var result = new List<SysColumn>();
            using (var conn = new SqlConnection(DbFactory.GetConnectionString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select table_name, column_name from information_schema.columns";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                            result.Add(new SysColumn() { TableName = rdr.GetString(0), ColumnName = rdr.GetString(1) });
                    }
                }
            }
            return result;
        }
    }
}
