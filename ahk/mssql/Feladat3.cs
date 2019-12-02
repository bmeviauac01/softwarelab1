using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ahk.adatvez.mssqldb;
using ahk.common;
using Microsoft.EntityFrameworkCore;

namespace adatvez
{
    internal class Feladat3
    {
        public const string AhkExerciseName = @"Feladat 3";

        public static void Execute()
        {
            var result = new AhkResult(AhkExerciseName);
            try
            {
                Console.WriteLine("Feladat 3 ellenorzese");

                test1(ref result);
                test2(ref result);
                test3(ref result);
            }
            finally
            {
                AhkResultWriter.WriteResult(result);
            }
        }

        private static void test1(ref AhkResult result)
        {
            bool columnExists = false;

            if (DbHelper.FindAndExecutionSolutionSqlFromFile(@"f3-oszlop.sql", @"f3-oszlop.sql", ref result))
            {
                if (SysObjectsHelper.ColumnExistsInTable("Szamla", "TetelSzam", ref result))
                {
                    columnExists = true;
                    result.AddPoints(1);
                }
            }

            // make sure this column exists; create if the student script has not created it
            if (!columnExists)
                DbHelper.ExecuteInstrumentationSql(@"alter table Szamla add TetelSzam int null");
        }

        private static void test2(ref AhkResult result)
        {
            // change some numbers in the database to make sure the required values are actually calculated and not hard-coded
            using (var db = DbFactory.GetDatabase())
            {
                var rnd = new Random(DateTime.UtcNow.Millisecond);
                foreach (var szt in db.SzamlaTetel)
                    szt.Mennyiseg += rnd.Next(0, 10);

                db.SaveChanges();
            }
            // insert a new szamla, so that if the script from the student explicitly handles existing ones, this will be skipped
            DbSampleData.InsertSampleSzamla();

            // run the student solution and verify outcome
            if (DbHelper.FindAndExecutionSolutionSqlFromFile(@"f3-kitolt.sql", @"f3-kitolt.sql", ref result))
            {
                if (!verifySzamlaTetelSzam())
                    result.AddProblem("TetelSzam ertek helytelen a kitoltes utan");
                else
                {
                    result.AddPoints(1);
                    result.Log("TetelSzam ertekek helyesek a kitoltes utan");
                }
            }
        }

        private static void test3(ref AhkResult result)
        {
            // reset the database to the known consistent state regarding the new column
            DbHelper.ExecuteInstrumentationSql(@"update Szamla set TetelSzam = (select sum(Mennyiseg) from SzamlaTetel where SzamlaTetel.SzamlaID = Szamla.ID)");

            // create trigger, fail early if the create operation fails
            if (!TextFileHelper.TryReadTextFile(@"f3-trigger.sql", @"f3-trigger.sql", ref result, out var sqlCommand))
                return;
            if (!DbHelper.ExecuteSolutionSql(@"f3-trigger.sql", sqlCommand, ref result))
                return;
            if (!sqlCommand.Contains("inserted", StringComparison.OrdinalIgnoreCase) || !sqlCommand.Contains("deleted", StringComparison.OrdinalIgnoreCase))
            {
                result.AddProblem("Trigger nem hasznalja a trigger tablakat");
                return;
            }


            int points = 0;

            // test - change a single record
            using (var db = DbFactory.GetDatabase())
            {
                db.SzamlaTetel.Random().Mennyiseg += 13;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "SzamlaTetel valtoztatas soran hiba (a trigger miatt ??!!)");
                    return;
                }
            }
            if (!verifySzamlaTetelSzam())
                result.AddProblem("TetelSzam ertek helytelen valtozas utan");
            else
            {
                ++points;
                result.Log("TetelSzam ertek kitoltes triggerrel teszt ok / 1");
            }


            // test - insert a new record
            using (var db = DbFactory.GetDatabase())
            {
                var itemToCopy = db.SzamlaTetel.Random();
                db.SzamlaTetel.Add(new ahk.adatvez.mssqldb.DatabaseContext.SzamlaTetel()
                {
                    Afakulcs = itemToCopy.Afakulcs,
                    MegrendelesTetelId = itemToCopy.MegrendelesTetelId,
                    Mennyiseg = itemToCopy.Mennyiseg * 2,
                    NettoAr = itemToCopy.NettoAr,
                    Nev = itemToCopy.Nev,
                });

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "SzamlaTetel valtoztatas soran hiba (a trigger miatt ??!!)");
                    return;
                }
            }
            if (!verifySzamlaTetelSzam())
                result.AddProblem("TetelSzam ertek helytelen beszuras utan");
            else
            {
                ++points;
                result.Log("TetelSzam ertek kitoltes triggerrel teszt ok / 2");
            }


            // test - delete a record
            using (var db = DbFactory.GetDatabase())
            {
                var itemToRemove = db.SzamlaTetel.Random();
                db.SzamlaTetel.Remove(itemToRemove);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "SzamlaTetel valtoztatas soran hiba (a trigger miatt ??!!)");
                    return;
                }
            }
            if (!verifySzamlaTetelSzam())
                result.AddProblem("TetelSzam ertek helytelen beszuras utan");
            else
            {
                ++points;
                result.Log("TetelSzam ertek kitoltes triggerrel teszt ok / 3");
            }


            // test - mass update
            using (var db = DbFactory.GetDatabase())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("update SzamlaTetel set Mennyiseg=Mennyiseg*2");
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "SzamlaTetel valtoztatas soran hiba (a trigger miatt ??!!)");
                    return;
                }
            }
            if (!verifySzamlaTetelSzam())
                result.AddProblem("TetelSzam ertek helytelen beszuras utan");
            else
            {
                ++points;
                result.Log("TetelSzam ertek kitoltes triggerrel teszt ok / 4");
            }


            // at last, check for screenshot
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"f3-trigger.png", @"f3-trigger.png", ref result);
            if (ok)
                result.AddPoints(points);
            else
                result.AddProblem($"Kepernyokep hianya miatt feladatresz nem ertekelt, egyebkent {points} pont lett volna");
        }

        private static bool verifySzamlaTetelSzam()
        {
            using (var db = DbFactory.GetDatabase())
            {
                foreach (var szamla in db.Szamla.Include(s => s.SzamlaTetel))
                {
                    int? szamlaTetel = getSzamlaTetelSzam(szamla.Id);

                    if (!szamlaTetel.HasValue)
                        return false;

                    if (szamla.SzamlaTetel.Sum(t => t.Mennyiseg) != szamlaTetel.Value)
                        return false;
                }
            }
            return true;
        }

        private static int? getSzamlaTetelSzam(int szamlaId)
        {
            using (var db = DbFactory.GetDatabase())
            {
                var @params = new[]
                {
                    new SqlParameter("@outres", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                db.Database.ExecuteSqlCommand($"select @outres=TetelSzam from szamla where id={szamlaId}", @params);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.

                return @params[0].Value as int?;
            }
        }
    }
}
