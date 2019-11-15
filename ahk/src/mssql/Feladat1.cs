﻿using System;
using System.Linq;

namespace adatvez
{
    internal class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1";

        public static void Execute()
        {
            var result = new AhkResult(AhkExerciseName);
            try
            {
                Console.WriteLine("Feladat 1 ellenorzese");

                DbHelper.ExecuteInstrumentationSql(@"IF OBJECT_ID('KategoriaSzulovel', 'V') IS NOT NULL DROP VIEW KategoriaSzulovel");
                DbHelper.ExecuteInstrumentationSql(@"create view KategoriaSzulovel as select k.Nev KategoriaNev, sz.Nev SzuloKategoriaNev from Kategoria k left outer join Kategoria sz on k.SzuloKategoria = sz.ID");
                result.Log("KategoriaSzulovel nezet letrehozva");

                test1(ref result);
                test2(ref result);
                test3(ref result);
                test4(ref result);
            }
            finally
            {
                AhkConsole.WriteResult(result);
            }
        }

        private static void test1(ref AhkResult result)
        {
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"Nezet tartalmat mutato kep", @"/megoldas/f1-nezet.png", ref result);
            if (ok)
                result.AddPoints(1);
        }

        private static void test2(ref AhkResult result)
        {
            var triggerName = @"KategoriaSzulovelBeszur";

            // execute script, fail early if it fails
            if (!DbHelper.FindAndExecutionSolutionSqlFromFile(@"f1-trigger.sql", @"/megoldas/f1-trigger.sql", ref result))
                return;

            // check if trigger exists, fail eraly if it does not
            if (!SysObjectsHelper.TriggerExistsWithName(triggerName, ref result))
                return;

            // test insert / 1

            var ujKategoriaNev = Guid.NewGuid().ToString().Replace("-", "").Substring(16);

            var kategoriaSzulovelDbContextOptions = DbFactory.GetDbOptions<KategoriaSzulovelDatabaseContext.KategoriaSzulovelDatabaseContext>();
            using (var db = new KategoriaSzulovelDatabaseContext.KategoriaSzulovelDatabaseContext(kategoriaSzulovelDbContextOptions))
            {
                db.KategoriaSzulovel.Add(new KategoriaSzulovelDatabaseContext.KategoriaSzulovel()
                {
                    KategoriaNev = ujKategoriaNev,
                    SzuloKategoriaNev = null
                });

                try
                {
                    db.SaveChanges();
                    result.Log("Beszuras KategoriaSzulovel nezeten keresztul sikeres / 1");
                    result.AddPoints(1);
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
                }
            }


            // test insert / 2

            ujKategoriaNev = Guid.NewGuid().ToString().Replace("-", "").Substring(16);

            string letezoKategoriaNev;
            using (var db = DbFactory.GetDatabase())
            {
                var rec = db.Kategoria.LastOrDefault();
                if (rec == null)
                    throw new Exception("Ures a Kategoria tabla. Mi tortent?");

                letezoKategoriaNev = rec.Nev;
            }

            using (var db = new KategoriaSzulovelDatabaseContext.KategoriaSzulovelDatabaseContext(kategoriaSzulovelDbContextOptions))
            {
                db.KategoriaSzulovel.Add(new KategoriaSzulovelDatabaseContext.KategoriaSzulovel()
                {
                    KategoriaNev = ujKategoriaNev,
                    SzuloKategoriaNev = letezoKategoriaNev
                });

                try
                {
                    db.SaveChanges();
                    result.Log("Beszuras KategoriaSzulovel nezeten keresztul sikeres / 2");
                    result.AddPoints(1);
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
                }
            }

            // test insert / 3

            ujKategoriaNev = Guid.NewGuid().ToString().Replace("-", "").Substring(16);
            var nemletezoKategoriaNev = Guid.NewGuid().ToString().Replace("-", "").Substring(16);

            using (var db = new KategoriaSzulovelDatabaseContext.KategoriaSzulovelDatabaseContext(kategoriaSzulovelDbContextOptions))
            {
                db.KategoriaSzulovel.Add(new KategoriaSzulovelDatabaseContext.KategoriaSzulovel()
                {
                    KategoriaNev = ujKategoriaNev,
                    SzuloKategoriaNev = nemletezoKategoriaNev
                });

                try
                {
                    db.SaveChanges();
                    result.AddProblem("Hibas viselkedes: nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul sikeres");
                }
                catch
                {
                    result.Log("Nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul helyesen hibat dob");
                    result.AddPoints(2);
                }
            }
        }

        private static void test3(ref AhkResult result)
        {
            int countKategoriaBefore;
            using (var db = DbFactory.GetDatabase())
                countKategoriaBefore = db.Kategoria.Count();

            // f1-trigger-teszt-ok.sql

            if (DbHelper.FindAndExecutionSolutionSqlFromFile(@"f1-trigger-teszt-ok.sql", @"/megoldas/f1-trigger-teszt-ok.sql", ref result))
            {
                int countKategoriaAfter;
                using (var db = DbFactory.GetDatabase())
                    countKategoriaAfter = db.Kategoria.Count();

                if (countKategoriaAfter == countKategoriaBefore + 1)
                {
                    result.Log("f1-trigger-teszt-ok.sql sikeresen beszurt egy Kategora rekordot");
                    result.AddPoints(1);
                }
                else
                {
                    result.AddProblem("f1-trigger-teszt-ok.sql nem szurt be Kategora rekordot");
                }
            }

            // f1-trigger-teszt-hiba.sql

            if (TextFileHelper.TryReadTextFileFromWellKnownPath(@"f1-trigger-teszt-hiba.sql", @"/megoldas/f1-trigger-teszt-hiba.sql", ref result, out var testTriggerHibaSql))
            {
                if (!(testTriggerHibaSql.Contains("insert", StringComparison.OrdinalIgnoreCase) && testTriggerHibaSql.Contains("KategoriaSzulovel", StringComparison.OrdinalIgnoreCase)))
                {
                    result.AddProblem("f1-trigger-teszt-hiba.sql nem szur be rekordot a nezetbe");
                }
                else
                {
                    var dummyResult = new AhkResult("f1-trigger-teszt-hiba.sql");
                    if (DbHelper.ExecuteSolutionSql("f1-trigger-teszt-hiba.sql", testTriggerHibaSql, ref dummyResult))
                    {
                        result.AddProblem("f1-trigger-teszt-hiba.sql futtatasa sikeres, pedig hibat kellett volna eredmenyeznie");
                    }
                    else
                    {
                        result.Log("f1-trigger-teszt-hiba.sql helyesen hibat jelzett");
                        result.AddPoints(1);
                    }
                }
            }
        }

        private static void test4(ref AhkResult result)
        {
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"Trigger kodjat mutato kep", @"/megoldas/f1-trigger.png", ref result);
            if (ok)
                result.AddPoints(1);
        }
    }
}
