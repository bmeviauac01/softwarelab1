using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ahk.common;
using Microsoft.EntityFrameworkCore;

namespace ahk.adatvez.mssqldb
{
    public static class DbHelper
    {
        public static bool FindAndExecutionSolutionSqlFromFile(string fileDescription, string fileFullPath, ref AhkResult result)
        {
            if (!TextFileHelper.TryReadTextFileFromWellKnownPath(fileDescription, fileFullPath, ref result, out var sqlCommand))
                return false;

            return ExecuteSolutionSql(fileDescription, sqlCommand, ref result);
        }

        public static bool ExecuteSolutionSql(string fileDescription, string sqlCommand, ref AhkResult result)
        {
            sqlCommand = SqlHelper.RemoveUseAndGoStatements(sqlCommand, ref result);

            using (var db = DbFactory.GetDatabase())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(sqlCommand);
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
                db.Database.ExecuteSqlCommand(sqlCommand);
        }

        public static bool FindAndExecutionSolutionSqlFromFileGetOutput(string fileDescription, string fileFullPath, out string output, ref AhkResult result)
        {
            output = null;

            if (!TextFileHelper.TryReadTextFileFromWellKnownPath(fileDescription, fileFullPath, ref result, out var sqlCommand))
                return false;

            sqlCommand = SqlHelper.RemoveUseAndGoStatements(sqlCommand, ref result);

            try
            {
                using (var db = DbFactory.GetDatabase())
                {
                    db.Database.OpenConnection();
                    var sqlConnection = (SqlConnection)db.Database.GetDbConnection();

                    var sb = new StringBuilder();
                    sqlConnection.InfoMessage += (s, a) => sb.AppendLine(a.Message);

                    db.Database.ExecuteSqlCommand(sqlCommand);
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
