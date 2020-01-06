using System;
using System.Linq;
using ahk.adatvez.mssqldb;
using ahk.common;

namespace adatvez
{
    internal class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1";

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine("Feladat 1 ellenorzese");

            DbHelper.ExecuteInstrumentationSql(@"IF OBJECT_ID('KategoriaSzulovel', 'V') IS NOT NULL DROP VIEW KategoriaSzulovel");
            DbHelper.ExecuteInstrumentationSql(@"create view KategoriaSzulovel as select k.Nev KategoriaNev, sz.Nev SzuloKategoriaNev from Kategoria k left outer join Kategoria sz on k.SzuloKategoria = sz.ID");
            ahkResult.Log("KategoriaSzulovel nezet letrehozva");

            test1(ahkResult);
            test2(ahkResult);

            bool ok = ScreenshotValidator.IsScreenshotPresent(@"Trigger kodjat mutato kep", @"f1.png", ahkResult);
            if (!ok)
                ahkResult.ResetPointToZero();
        }

        private static void test1(AhkResult ahkResult)
        {
            var triggerName = @"KategoriaSzulovelBeszur";

            // execute script, fail early if it fails
            if (!DbHelper.FindAndExecutionSolutionSqlFromFile(@"f1-trigger.sql", @"f1-trigger.sql", ahkResult))
                return;

            // check if trigger exists, fail eraly if it does not
            if (!SysObjectsHelper.TriggerExistsWithName(triggerName, ahkResult))
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
                    ahkResult.Log("Beszuras KategoriaSzulovel nezeten keresztul sikeres");
                    ahkResult.AddPoints(1);
                }
                catch (Exception ex)
                {
                    ahkResult.AddProblem(ex, "Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
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
                    ahkResult.Log("Beszuras KategoriaSzulovel nezeten keresztul sikeres");
                    ahkResult.AddPoints(1);
                }
                catch (Exception ex)
                {
                    ahkResult.AddProblem(ex, "Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
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
                    ahkResult.AddProblem("Hibas viselkedes: nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul sikeres");
                }
                catch
                {
                    ahkResult.Log("Nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul helyesen hibat dob");
                    ahkResult.AddPoints(2);
                }
            }
        }

        private static void test2(AhkResult ahkResult)
        {
            int countKategoriaBefore;
            using (var db = DbFactory.GetDatabase())
                countKategoriaBefore = db.Kategoria.Count();

            // f1-trigger-teszt-ok.sql

            if (DbHelper.FindAndExecutionSolutionSqlFromFile(@"f1-trigger-teszt-ok.sql", @"f1-trigger-teszt-ok.sql", ahkResult))
            {
                int countKategoriaAfter;
                using (var db = DbFactory.GetDatabase())
                    countKategoriaAfter = db.Kategoria.Count();

                if (countKategoriaAfter == countKategoriaBefore + 1)
                {
                    ahkResult.Log("f1-trigger-teszt-ok.sql sikeresen beszurt egy Kategora rekordot");
                    ahkResult.AddPoints(2);
                }
                else
                {
                    ahkResult.AddProblem("f1-trigger-teszt-ok.sql nem szurt be Kategora rekordot");
                }
            }

            // f1-trigger-teszt-hiba.sql

            if (TextFileHelper.TryReadTextFile(@"f1-trigger-teszt-hiba.sql", @"f1-trigger-teszt-hiba.sql", ahkResult, out var testTriggerHibaSql))
            {
                if (!(testTriggerHibaSql.Contains("insert", StringComparison.OrdinalIgnoreCase) && testTriggerHibaSql.Contains("KategoriaSzulovel", StringComparison.OrdinalIgnoreCase)))
                {
                    ahkResult.AddProblem("f1-trigger-teszt-hiba.sql nem szur be rekordot a nezetbe");
                }
                else
                {
                    var dummyResult = new AhkResult("f1-trigger-teszt-hiba.sql");
                    if (DbHelper.ExecuteSolutionSql("f1-trigger-teszt-hiba.sql", testTriggerHibaSql, dummyResult))
                    {
                        ahkResult.AddProblem("f1-trigger-teszt-hiba.sql futtatasa sikeres, pedig hibat kellett volna eredmenyeznie");
                    }
                    else
                    {
                        ahkResult.Log("f1-trigger-teszt-hiba.sql helyesen hibat jelzett");
                        ahkResult.AddPoints(2);
                    }
                }
            }
        }
    }
}
