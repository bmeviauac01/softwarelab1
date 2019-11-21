﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace adatvez
{
    internal class Feladat4
    {
        public const string AhkExerciseName = @"imsc@Feladat 4";

        public static void Execute()
        {
            var result = new AhkResult(AhkExerciseName);
            try
            {
                Console.WriteLine("Feladat 4 ellenorzese");

                test(ref result);
            }
            finally
            {
                AhkConsole.WriteResult(result);
            }
        }

        private static void test(ref AhkResult result)
        {
            // chane some categories, and get expected outcome
            var productToRootCategory = new Dictionary<int, int>();
            using (var db = DbFactory.GetDatabase())
            {
                // clear parent of a random category
                db.Kategoria.Random(k => k.SzuloKategoria != null).SzuloKategoria = null;
                db.SaveChanges();

                // add a new sub-category with products
                db.Kategoria.Add(new DatabaseContext.Kategoria()
                {
                    Nev = "Uj kat",
                    SzuloKategoriaNavigation = db.Kategoria.Random(),
                    Termek = new[]
                    {
                        DbSampleData.CreateNewProduct(),
                        DbSampleData.CreateNewProduct()
                    }
                });
                db.SaveChanges();

                // gather the expected result
                foreach (var ter in db.Termek.Include(t => t.Kategoria).ThenInclude(k => k.SzuloKategoriaNavigation))
                {
                    var rootKat = ter.Kategoria;
                    while (rootKat.SzuloKategoriaNavigation != null)
                        rootKat = rootKat.SzuloKategoriaNavigation;

                    productToRootCategory[ter.Id] = rootKat.Id;
                }
            }


            // execute solution, fail early if it failes
            if (!DbHelper.FindAndExecutionSolutionSqlFromFile(@"f4.sql", @"/megoldas/f4.sql", ref result))
                return;

            // test outcome
            using (var db = DbFactory.GetDatabase())
            {
                foreach (var k in db.Kategoria)
                {
                    if (k.SzuloKategoria != null)
                    {
                        result.AddProblem("SzuloKategoria kitoltve maradt");
                        return;
                    }

                    // This check is not completely right: there can be a top level category group that remains even after the changes with no product
                    // Here productToRootCategory only contains ids of existing products
                    //if (!productToRootCategory.ContainsValue(k.Id))
                    //{
                    //    result.AddProblem("Helytelen megmaradt legfelso kategoria");
                    //    return;
                    //}
                }

                foreach (var ter in db.Termek)
                {
                    if (ter.KategoriaId != productToRootCategory[ter.Id])
                    {
                        result.AddProblem("Termek nem lett atsorolva a legfelso szintu kategoriaba");
                        return;
                    }
                }
            }

            // at last, check for screenshot
            bool ok = ScreenshotValidator.IsScreenshotPresent(@"f4.png", @"/megoldas/f4.png", ref result);
            if (ok)
            {
                result.AddPoints(3);
                result.Log("Kategoria hierarchia kilapitas ok");
            }
            else
                result.AddProblem($"Kepernyokep hianya miatt feladatresz nem ertekelt");
        }
    }
}