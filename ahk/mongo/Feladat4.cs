using adatvez.DAL;
using adatvez.Helpers;
using ahk.common;
using MongoLabor.DAL.Entities;
using System;
using System.Linq;

namespace adatvez
{
    internal static class Feladat4
    {
        public const string AhkExerciseName = @"Feladat 4 - Exercise 4";

        private static readonly Termek[] termekek = new[]
        {
            RandomEntityFactory.CreateRandomTermek(),
            RandomEntityFactory.CreateRandomTermek(),
            RandomEntityFactory.CreateRandomTermek(),
        };

        private static readonly Vevo vevo1 = RandomEntityFactory.CreateRandomVevo(3);
        private static readonly Vevo vevo2 = RandomEntityFactory.CreateRandomVevo(1);

        private static Megrendeles megrendeles1;
        private static Megrendeles megrendeles2;

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 4 ###### Exercise 4 ######");

            try
            {
                if (prepareDb(ahkResult))
                {
                    var repository = new MongoLabor.DAL.AdatvezRepository(DbFactory.Database);

                    testList(repository, ahkResult);

                    if (!ScreenshotValidator.IsScreenshotPresent("f4-vevok.png", "f4-vevok.png", ahkResult))
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
                DbFactory.TermekCollection.InsertMany(termekek);

                DbFactory.VevoCollection.InsertOne(vevo1);
                DbFactory.VevoCollection.InsertOne(vevo2);

                megrendeles1 = RandomEntityFactory.CreateRandomMegrendeles(vevo1.ID, vevo1.KozpontiTelephelyID, DateTime.UtcNow, termekek[0], termekek[1]);
                megrendeles2 = RandomEntityFactory.CreateRandomMegrendeles(vevo1.ID, vevo1.KozpontiTelephelyID, DateTime.UtcNow, termekek[2]);
                DbFactory.MegrendelesCollection.InsertOne(megrendeles1);
                DbFactory.MegrendelesCollection.InsertOne(megrendeles2);

                ahkResult.Log("Termek, Vevo and Megrendeles insert ok");
                return true;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "Db inicializalas / Termek, Vevo es Megrendeles mentese sikertelen. Failed to initialize Db / failed to add Product, Customer and Order to database.");
                return false;
            }
        }

        private static void testList(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.ListVevok)}";

            var repoListResult = Op.Func(() => repository.ListVevok().ToArray()).TryRunOperationAndCheckLength(5, ahkResult, operationName);
            if (!repoListResult.Success)
                return;

            var repoItemResult = repoListResult.TryFindItem(
                v => v.Nev == vevo1.Nev && v.IR == vevo1.Telephelyek[2].IR && v.Varos == vevo1.Telephelyek[2].Varos && v.Utca == vevo1.Telephelyek[2].Utca
                && v.OsszMegrendeles == megrendeles1.MegrendelesTetelek.Concat(megrendeles2.MegrendelesTetelek).Sum(mt => mt.Mennyiseg * mt.NettoAr), ahkResult, $"{operationName} (megrendelessel - with orders)");
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(2);
                ahkResult.Log($"{operationName} - megrendelessel ok. {operationName} - with order ok");
            }

            repoItemResult = repoListResult.TryFindItem(
                v => v.Nev == vevo2.Nev && v.IR == vevo2.Telephelyek[0].IR && v.Varos == vevo2.Telephelyek[0].Varos && v.Utca == vevo2.Telephelyek[0].Utca
                && v.OsszMegrendeles == null, ahkResult, $"{operationName} (megrendeles nelkul - without orders)");
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(2);
                ahkResult.Log($"{operationName} - megrendeles nelkul ok. {operationName} - without orders ok");
            }
        }
    }
}
