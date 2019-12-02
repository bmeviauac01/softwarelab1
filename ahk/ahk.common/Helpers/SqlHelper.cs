using System;
using System.IO;
using System.Text;

namespace ahk.common
{
    public static class SqlHelper
    {
        public static string RemoveUseAndGoStatements(string sql, AhkResult result)
        {
            if (string.IsNullOrEmpty(sql))
                return sql;

            var sqlResult = new StringBuilder();
            using (var reader = new StringReader(sql))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("go", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Log("go utasiatassal kezdodo sor eltavolitva");
                        continue;
                    }
                    if (line.StartsWith("use", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Log("use utasitassal kezdodo sor eltavolitva");
                        continue;
                    }
                    if (line.StartsWith("[use]", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Log("use utasitassal kezdodo sor eltavolitva");
                        continue;
                    }

                    sqlResult.AppendLine(line);
                }
            }

            return sqlResult.ToString();
        }
    }
}
