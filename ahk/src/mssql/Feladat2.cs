using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace adatvez
{
    internal class Feladat2
    {
        public const string AhkExerciseName = @"Feladat 2";

        private const string procName = @"SzamlaEllenoriz";

        public static void Execute()
        {
            var result = new AhkResult(AhkExerciseName);
            try
            {
                Console.WriteLine("Feladat 2 ellenorzese");

                if (createStoredProc(ref result))
                {
                    test1(ref result);
                    test2(ref result);
                }
            }
            finally
            {
                AhkConsole.WriteResult(result);
            }
        }

        private static bool createStoredProc(ref AhkResult result)
        {
            // execute script, fail early if it fails
            if (!DbHelper.FindAndExecutionSolutionSqlFromFile(@"f2-eljaras.sql", @"/megoldas/f2-eljaras.sql", ref result))
                return false;

            // check if procedure exists, fail early if it does not
            if (!SysObjectsHelper.StoredProcedureExistsWithName(procName, ref result))
                return false;

            return true;
        }

        private static void test1(ref AhkResult result)
        {
            int points = 0;

            // Ensure that the items to check are identical
            ensureDbHasNoDiscrepancies();

            // Test when everything is ok
            if (runStoredProcGetReturnValueAndOutput(1, out var procReturn1, out string procOutput1, ref result))
            {
                var returnValueOk = procReturn1.HasValue && procReturn1.Value == 0;
                var logOk = string.IsNullOrEmpty(procOutput1.Trim());

                if (returnValueOk)
                {
                    ++points;
                    result.Log("Eljaras teszt ok / 1");
                }
                else
                    result.AddProblem("Eljaras visszateresi erteke helytelen, ha nincs hiba");

                if (logOk)
                {
                    ++points;
                    result.Log("Eljaras teszt ok / 2");
                }
                else
                    result.AddProblem("Eljaras kimenetre irt szovege nem ures, pedig nincs hiba");
            }

            // Ensure two discrepancies
            var problemProductNames = ensureDbWithDiscrepancies(out var szamlaIdWithDiscrepancies);

            // Test when everything is ok
            if (runStoredProcGetReturnValueAndOutput(szamlaIdWithDiscrepancies, out var procReturn2, out string procOutput2, ref result))
            {
                var returnValueOk = procReturn2.HasValue && procReturn2.Value == 1;
                var logOk = problemProductNames.All(name => procOutput2.Contains(name));

                if (returnValueOk)
                {
                    ++points;
                    result.Log("Eljaras teszt ok / 3");
                }
                else
                    result.AddProblem("Eljaras visszateresi erteke helytelen, ha hiba van az adatokban");

                if (logOk)
                {
                    ++points;
                    result.Log("Eljaras teszt ok / 4");
                }
                else
                    result.AddProblem("Eljaras kimenetre irt szovege nem tartalmazza a hibat");
            }

            bool ok = ScreenshotValidator.IsScreenshotPresent(@"f2-eljaras.png", @"/megoldas/f2-eljaras.png", ref result);
            if (ok)
                result.AddPoints(points);
            else
                result.AddProblem($"Kepernyokep hianya miatt feladatresz nem ertekelt, egyebkent {points} pont lett volna");
        }

        private static void test2(ref AhkResult result)
        {
            var problemProductNames = ensureDbWithDiscrepancies(out var szamlaIdWithDiscrepancies);

            if (DbHelper.FindAndExecutionSolutionSqlFromFileGetOutput("f2-futtatas.sql", @"/megoldas/f2-futtatas.sql", out var output, ref result))
            {
                if (output.Contains("Helyes szamla"))
                {
                    result.AddPoints(1);
                    result.Log("Osszes szamla ellenorzese teszt ok / 1");
                }
                else
                    result.AddProblem("Az osszes szamla ellenorzese soran kellene legyen helyes szamla is");

                var logContainsInconsistentProductNames = problemProductNames.All(name => output.Contains(name));
                if (logContainsInconsistentProductNames)
                {
                    result.AddPoints(1);
                    result.Log("Osszes szamla ellenorzese teszt ok / 2");
                }
                else
                    result.AddProblem("Az osszes szamla ellenorzese soran nem jelentek meg a hibas termekek a kimenetben");
            }
        }

        private static string[] ensureDbWithDiscrepancies(out int szamlaid)
        {
            // First reset to consistent state
            ensureDbHasNoDiscrepancies();

            // Then change something
            var problemProductNames = new List<string>();
            using (var db = DbFactory.GetDatabase())
            {
                var szamla = db.Szamla.Random(sz => sz.SzamlaTetel.Any());
                szamlaid = szamla.Id;

                foreach (var tetel in db.SzamlaTetel.Where(szt => szt.SzamlaId == szamla.Id).Include(szt => szt.MegrendelesTetel).ThenInclude(mt => mt.Termek))
                {
                    problemProductNames.Add(tetel.MegrendelesTetel.Termek.Nev);
                    tetel.Mennyiseg = tetel.MegrendelesTetel.Mennyiseg + 2;
                }

                db.SaveChanges();
            }
            return problemProductNames.ToArray();
        }

        private static void ensureDbHasNoDiscrepancies()
        {
            using (var db = DbFactory.GetDatabase())
            {
                foreach (var szt in db.SzamlaTetel.Include(szt => szt.MegrendelesTetel))
                {
                    if (szt.Mennyiseg != szt.MegrendelesTetel.Mennyiseg)
                        szt.Mennyiseg = szt.MegrendelesTetel.Mennyiseg;
                }

                db.SaveChanges();
            }
        }

        private static bool runStoredProcGetReturnValueAndOutput(int szamlaId, out int? procedureReturnValue, out string procedureOutput, ref AhkResult result)
        {
            try
            {
                using (var db = DbFactory.GetDatabase())
                {
                    db.Database.OpenConnection();
                    var sqlConnection = (SqlConnection)db.Database.GetDbConnection();

                    var sb = new StringBuilder();
                    sqlConnection.InfoMessage += (s, a) => sb.AppendLine(a.Message);

                    var @params = new[]
                    {
                        new SqlParameter("@szamlaaz", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = szamlaId },
                        new SqlParameter("@returnVal", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                    db.Database.ExecuteSqlCommand($"exec @returnVal={procName} @szamlaaz", @params);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.

                    procedureReturnValue = @params[1].Value as int?;
                    procedureOutput = sb.ToString();
                }

                return true;
            }
            catch (Exception ex)
            {
                result.AddProblem(ex, $"{procName} eljaras futtatasa soran hiba tortent");

                procedureReturnValue = null;
                procedureOutput = null;
                return false;
            }
        }
    }
}
