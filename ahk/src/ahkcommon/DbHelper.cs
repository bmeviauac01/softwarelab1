using System;
using Microsoft.EntityFrameworkCore;

namespace adatvez
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
    }
}
