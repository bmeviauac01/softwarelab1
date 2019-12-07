using ahk.common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ahk.adatvez.mssqldb
{
    public static class DbHelper
    {
        public static bool FindAndExecutionSolutionSqlFromFile(string fileDescription, string fileFullPath, AhkResult result)
        {
            if (!TextFileHelper.TryReadTextFile(fileDescription, fileFullPath, result, out var sqlCommand))
                return false;

            return ExecuteSolutionSql(fileDescription, sqlCommand, result);
        }

        public static bool ExecuteSolutionSql(string fileDescription, string sqlCommand, AhkResult result)
        {
            sqlCommand = SqlHelper.RemoveUseAndGoStatements(sqlCommand, result);

            using (var db = DbFactory.GetDatabase())
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sqlCommand);
                    result.Log($"{fileDescription} sikeresen lefuttatva");
                    return true;
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, $"Hiba a {fileDescription} futtatasa soran: {ex.Message}");
                    return false;
                }
            }
        }

        public static void ExecuteInstrumentationSql(string sqlCommand)
        {
            using (var db = DbFactory.GetDatabase())
                db.Database.ExecuteSqlRaw(sqlCommand);
        }

        public static bool FindAndExecutionSolutionSqlFromFileGetOutput(string fileDescription, string fileFullPath, out string output, AhkResult result)
        {
            output = null;

            if (!TextFileHelper.TryReadTextFile(fileDescription, fileFullPath, result, out var sqlCommand))
                return false;

            sqlCommand = SqlHelper.RemoveUseAndGoStatements(sqlCommand, result);

            try
            {
                using (var db = DbFactory.GetDatabase())
                {
                    db.Database.OpenConnection();
                    var sqlConnection = (SqlConnection)db.Database.GetDbConnection();

                    var sb = new StringBuilder();
                    sqlConnection.InfoMessage += (s, a) => sb.AppendLine(a.Message);

                    db.Database.ExecuteSqlRaw(sqlCommand);
                    result.Log($"{fileDescription} sikeresen lefuttatva");

                    output = sb.ToString();
                }

                return true;
            }
            catch (Exception ex)
            {
                result.AddProblem(ex, $"Hiba a {fileDescription} futtatasa soran: {ex.Message}");
                return false;
            }
        }

        public static T Random<T>(this IQueryable<T> items, Expression<Func<T, bool>> filter = null)
        {
            var allItems = filter == null ? items.ToList() : items.Where(filter).ToList();

            if (allItems.Count == 0)
                return default;

            var random = new Random(DateTime.UtcNow.Millisecond);
            var randomIndex = random.Next(0, allItems.Count);
            return allItems[randomIndex];
        }
    }
}
