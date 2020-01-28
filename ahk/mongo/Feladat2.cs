using adatvez.DAL;
using adatvez.DAL.Entities;
using ahk.common;
using ahk.common.Helpers;
using System;
using System.Linq;

namespace adatvez
{
    internal static class Feladat2
    {
        public const string AhkExerciseName = @"Feladat 2 - Exercise 2";

        private static readonly Kategoria szuloKategoria = new Kategoria
        {
            Nev = Guid.NewGuid().ToString(),
        };

        private static readonly Kategoria emptyKategoria = new Kategoria
        {
            Nev = Guid.NewGuid().ToString(),
        };

        private static readonly Kategoria kategoria = new Kategoria
        {
            Nev = Guid.NewGuid().ToString(),
        };

        private static readonly int termekCount = RandomHelper.GetRandomValue(3, 11);

        private static readonly Termek termek = new Termek
        {
            Nev = Guid.NewGuid().ToString(),
            NettoAr = 432.1,
            Raktarkeszlet = 10,
        };

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 2 ###### Exercise 2 ######");

            try
            {
                if (prepareDb(ahkResult))
                {
                    var repository = new MongoLabor.DAL.AdatvezRepository(DbFactory.Database);

                    testList(repository, ahkResult);

                    if (!ScreenshotValidator.IsScreenshotPresent("f2-kategoriak.png", "f2-kategoriak.png", ahkResult))
                        ahkResult.ResetPointToZero();
                }
            }
            catch (MissingMethodException ex) // expected problem, violates contract, solution evaluation ignored
            {
                ahkResult.AddProblem(ex, "Nem megengedett kodot valtoztattal. Changed code that you should not have.");
            }
            catch (TypeLoadException ex) // expected problem, violates contract, solution evaluation ignored
            {
                ahkResult.AddProblem(ex, "Nem megengedett kodot valtoztattal. Changed code that you should not have.");
            }
        }

        private static bool prepareDb(AhkResult ahkResult)
        {
            try
            {
                DbFactory.KategoriaCollection.InsertOne(szuloKategoria);

                emptyKategoria.SzuloKategoriaID = szuloKategoria.ID;
                kategoria.SzuloKategoriaID = szuloKategoria.ID;
                DbFactory.KategoriaCollection.InsertOne(emptyKategoria);
                DbFactory.KategoriaCollection.InsertOne(kategoria);

                termek.KategoriaID = kategoria.ID;
                for (int i = 0; i < termekCount; i++)
                {
                    DbFactory.TermekCollection.InsertOne(termek);
                    termek.ID = default;
                }


                ahkResult.Log("Kategoria and Termek insert ok");
                return true;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "Db inicializalas / Termek mentese sikertelen. Failed to initialize Db / failed to add Product to database.");
                return false;
            }
        }

        private static void testList(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.ListTermekek)}";

            var repoListResult = Op.Func(() => repository.ListKategoriak().ToArray()).TryRunOperationAndCheckLength(23, ahkResult, operationName);
            if (!repoListResult.Success)
                return;

            var repoItemResult = repoListResult.TryFindItem(k => k.Nev == szuloKategoria.Nev && k.SzuloKategoriaNev == null && k.TermekDarab == 0, ahkResult, $"{operationName} (szulo - parent)");
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} - szulo kategoria ok. {operationName} - parent category ok");
            }

            repoItemResult = repoListResult.TryFindItem(k => k.Nev == emptyKategoria.Nev && k.SzuloKategoriaNev == szuloKategoria.Nev && k.TermekDarab == 0, ahkResult, $"{operationName} (ures - empty)");
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} - ures kategoria ok. {operationName} - empty category ok");
            }

            repoItemResult = repoListResult.TryFindItem(k => k.Nev == kategoria.Nev && k.SzuloKategoriaNev == szuloKategoria.Nev && k.TermekDarab == termekCount, ahkResult, operationName);
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(2);
                ahkResult.Log($"{operationName} ok");
            }
        }
    }
}
