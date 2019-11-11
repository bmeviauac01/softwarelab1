using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace adatvez
{
    internal class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1";

        public static void Execute()
        {
            Console.WriteLine("Feladat 1 ellenorzese");

            using (var db = DbFactory.GetDatabase())
            {
                db.Database.ExecuteSqlCommand(@"create view KategoriaSzulovel as select k.Nev KategoriaNev, sz.Nev SzuloKategoriaNev from Kategoria k left outer join Kategoria sz on k.SzuloKategoria = sz.ID");
                Console.WriteLine("KategoriaSzulovel nezet letrehozva");
            }

            var points = 0;
            var problems = new List<string>();

            test1(ref points, ref problems);
            test2(ref points, ref problems);
            test3(ref points, ref problems);
            test4(ref points, ref problems);

            AhkConsole.WriteResult(AhkExerciseName, points, problems.ToArray());
        }

        private static void test1(ref int points, ref List<string> problems)
        {
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"Nezet tartalmat mutato", @"/megoldas/f1-nezet.png", ref problems);
            if (ok)
                points += 1;
        }

        private static void test2(ref int points, ref List<string> problems)
        {
            var triggerName = @"KategoriaSzulovelBeszur";

            // check sql file existance and content
            if (!TextFileHelper.TryReadTextFileFromWellKnownPath(@"/megoldas/f1-trigger.sql", @"f1-trigger.sql", ref problems, out var createTriggerSql))
                return;

            // execute script, fail early if it fails

            using (var db = DbFactory.GetDatabase())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(createTriggerSql);
                    Console.WriteLine("f1-trigger.sql sikeresen lefuttatva");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    problems.Add("Hiba az f1-trigger.sql script futtatasa soran");
                    return;
                }
            }

            // check if trigger exists

            var sysobjectsDbContextOptions = DbFactory.GetDbOptions<SysObjectsDbContext.SysObjectsDbContext>();
            using (var db = new SysObjectsDbContext.SysObjectsDbContext(sysobjectsDbContextOptions))
            {
                var trigger = db.ListSysObjects().FirstOrDefault(obj => obj.Type.Equals("TR", StringComparison.OrdinalIgnoreCase) && obj.Name.Equals(triggerName, StringComparison.OrdinalIgnoreCase));
                if (trigger == null)
                {
                    problems.Add("Nem letezik a KategoriaSzulovelBeszur trigger");
                    return;
                }
            }

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
                    Console.WriteLine("Beszuras KategoriaSzulovel nezeten keresztul sikeres");
                    points += 1;
                }
                catch (Exception ex)
                {
                    problems.Add("Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
                    problems.Add(ex.Message);
                }
            }


            // test insert / 2

            ujKategoriaNev = Guid.NewGuid().ToString().Replace("-", "").Substring(16);

            string letezoKategoriaNev;
            using (var db = DbFactory.GetDatabase())
            {
                var rec = db.Kategoria.LastOrDefault();
                if (rec == null)
                {
                    problems.Add("Ures a Kategoria tabla. Mi tortent?");
                    return;
                }

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
                    Console.WriteLine("Beszuras KategoriaSzulovel nezeten keresztul sikeres");
                    points += 1;
                }
                catch (Exception ex)
                {
                    problems.Add("Beszuras KategoriaSzulovel nezeten keresztul nem sikerul (talan a trigger miatt?)");
                    problems.Add(ex.Message);
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

                    problems.Add("Nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul sikeres");
                }
                catch
                {
                    Console.WriteLine("Nem letezo szulo kategoriaval beszuras KategoriaSzulovel nezeten keresztul helyesen hibat dob");
                    points += 2;
                }
            }
        }

        private static void test3(ref int points, ref List<string> problems)
        {
            // f1-trigger-teszt-ok.sql

            if (TextFileHelper.TryReadTextFileFromWellKnownPath(@"/megoldas/f1-trigger-teszt-ok.sql", @"f1-trigger-teszt-ok.sql", ref problems, out var testTriggerOkSql))
            {
                using (var db = DbFactory.GetDatabase())
                {
                    try
                    {
                        var countKategoriaBefore = db.Kategoria.Count();

                        db.Database.ExecuteSqlCommand(testTriggerOkSql);

                        Console.WriteLine("f1-trigger-teszt-ok.sql futtatasa sikeres");

                        var countKategoriaAfter = db.Kategoria.Count();

                        if (countKategoriaAfter == countKategoriaBefore + 1)
                        {
                            Console.WriteLine("f1-trigger-teszt-ok.sql sikeresen beszurt egy Kategora rekordot");
                            points += 1;
                        }
                        else
                        {
                            problems.Add("f1-trigger-teszt-ok.sql nem szurt be Kategora rekordot");
                        }
                    }
                    catch (Exception ex)
                    {
                        problems.Add("f1-trigger-teszt-ok.sql futtatasa nem sikerul (talan a trigger miatt?)");
                        problems.Add(ex.Message);
                    }
                }
            }

            // f1-trigger-teszt-hiba.sql

            if (TextFileHelper.TryReadTextFileFromWellKnownPath(@"/megoldas/f1-trigger-teszt-hiba.sql", @"f1-trigger-teszt-hiba.sql", ref problems, out var testTriggerHibaSql))
            {
                if (!(testTriggerHibaSql.Contains("insert", StringComparison.OrdinalIgnoreCase) && testTriggerHibaSql.Contains("KategoriaSzulovel", StringComparison.OrdinalIgnoreCase)))
                {
                    problems.Add("f1-trigger-teszt-hiba.sql nem szur be rekordot a nezetbe");
                }
                else
                {
                    using (var db = DbFactory.GetDatabase())
                    {
                        try
                        {
                            var countKategoriaBefore = db.Kategoria.Count();

                            db.Database.ExecuteSqlCommand(testTriggerHibaSql);

                            problems.Add("f1-trigger-teszt-hiba.sql futtatasa sikeres, pedig hibat kellett volna eredmenyeznie");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("f1-trigger-teszt-hiba.sql helyesen hibat jelzett");
                            points += 1;
                        }
                    }
                }
            }
        }

        private static void test4(ref int points, ref List<string> problems)
        {
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"Trigger kodjat mutato", @"/megoldas/f1-trigger.png", ref problems);
            if (ok)
                points += 1;
        }
    }
}
