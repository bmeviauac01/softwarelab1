using adatvez.DAL;
using adatvez.DAL.Entities;
using ahk.common;
using ahk.common.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace adatvez
{
    internal static class Feladat3
    {
        public const string AhkExerciseName = @"Feladat 3 - Exercise 3";

        private static readonly Termek termek1 = RandomEntityFactory.CreateRandomTermek();
        private static readonly Termek termek2 = RandomEntityFactory.CreateRandomTermek();

        private static Megrendeles megrendeles;
        private static double? osszErtek;

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 3 ###### Exercise 3 ######");

            try
            {
                if (prepareDb(ahkResult))
                {
                    var repository = new MongoLabor.DAL.AdatvezRepository(DbFactory.Database);

                    testList(repository, ahkResult);
                    testFind(repository, ahkResult);
                    testInsert(repository, ahkResult);
                    testUpdate(repository, ahkResult);
                    testDelete(repository, ahkResult);

                    if (!ScreenshotValidator.IsScreenshotPresent("f3-megrendelesek.png", "f3-megrendelesek.png", ahkResult))
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
                DbFactory.TermekCollection.InsertOne(termek1);
                DbFactory.TermekCollection.InsertOne(termek2);

                megrendeles = RandomEntityFactory.CreateRandomMegrendeles(vevoId: ObjectId.Parse("5d7e42adcffa8e1b64f7dbb9"), telephelyId: ObjectId.Parse("5d7e42adcffa8e1b64f7dbba"), termek1, termek2);
                DbFactory.MegrendelesCollection.InsertOne(megrendeles);
                osszErtek = megrendeles.MegrendelesTetelek.Sum(mt => mt.Mennyiseg * mt.NettoAr);

                ahkResult.Log("Termek and Megrendeles insert ok");
                return true;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "Db inicializalas / Termek es Megrendeles mentese sikertelen. Failed to initialize Db / failed to add Product and Order to database.");
                return false;
            }
        }

        private static void testList(MongoLabor.DAL.IAdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.ListMegrendelesek)}";

            var successful = true;
            var repoListResult = Op.Func(() => repository.ListMegrendelesek(null).ToArray()).TryRunOperationAndCheckLength(5, ahkResult, $"{operationName} - null");
            if (repoListResult.Success)
                ahkResult.Log($"{operationName} - nincs szures (null) ok. {operationName} - no filtering (null) ok");
            else
                successful = false;

            repoListResult = Op.Func(() => repository.ListMegrendelesek(string.Empty).ToArray()).TryRunOperationAndCheckLength(5, ahkResult, $"{operationName} - empty");
            if (repoListResult.Success)
                ahkResult.Log($"{operationName} - nincs szures (ures) ok. {operationName} - no filtering (empty) ok");
            else
                successful = false;

            repoListResult = Op.Func(() => repository.ListMegrendelesek(megrendeles.Statusz).ToArray()).TryRunOperationAndCheckLength(1, ahkResult, $"{operationName} - existing");
            if (repoListResult.Success)
            {
                var repoItemResult = repoListResult.TryFindItem(m => areEqual(megrendeles, m) && m.OsszErtek == osszErtek, ahkResult, $"{operationName} - existing");
                if (repoItemResult.Success)
                    ahkResult.Log($"{operationName} - letezo statusz ok. {operationName} - existing status filtering ok");
                else
                    successful = false;
            }
            else
            {
                successful = false;
            }

            repoListResult = Op.Func(() => repository.ListMegrendelesek("not-existing-status").ToArray()).TryRunOperationAndCheckLength(0, ahkResult, $"{operationName} - non-esistent");
            if (repoListResult.Success)
                ahkResult.Log($"{operationName} - nem letezo statusz ok. {operationName} - not-existent status ok");
            else
                successful = false;

            if (successful)
                ahkResult.AddPoints(1);
        }

        private static void testFind(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.FindMegrendeles)}";

            var successful = true;
            var repoFindResult = Op.Func(() => repository.FindMegrendeles(megrendeles.ID.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoFindResult.Success && areEqual(megrendeles, repoFindResult.Value) && repoFindResult.Value.ID == megrendeles.ID.ToString() && repoFindResult.Value.OsszErtek == osszErtek)
            {
                ahkResult.Log($"{operationName} - letezo megrendeles ok. {operationName} - existing order ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} nem az elvart ertekkel ter vissza. {operationName} does not return the expected result");
            }

            repoFindResult = Op.Func(() => repository.FindMegrendeles(ObjectId.Empty.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoFindResult.Success && repoFindResult.Value == null)
            {
                ahkResult.Log($"{operationName} - nem letezo megrendeles ok. {operationName} - non-existent order ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} nem null ertekkel ter vissza. {operationName} does not return null result");
            }

            if (successful)
                ahkResult.AddPoints(1);
        }

        private static void testInsert(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.InsertMegrendeles)}";

            var termekToInsert = new MongoLabor.Models.Termek
            {
                ID = termek1.ID.ToString(),
                Nev = termek1.Nev,
                NettoAr = termek1.NettoAr,
                Raktarkeszlet = termek1.Raktarkeszlet,
            };
            var megrendelesToInsert = new MongoLabor.Models.Megrendeles
            {
                Statusz = Guid.NewGuid().ToString(),
                FizetesMod = Guid.NewGuid().ToString(),
                Datum = DateTime.UtcNow.AddMinutes(RandomHelper.GetRandomValue(25, 55)),
                Hatarido = DateTime.UtcNow.AddDays(RandomHelper.GetRandomValue(1, 8)),
            };
            var mennyiseg = RandomHelper.GetRandomValue(50, 100);

            var repoInsertResult = Op.Func(() => repository.InsertMegrendeles(megrendelesToInsert, termekToInsert, mennyiseg)).TryRunOperation(ahkResult, operationName);
            if (!repoInsertResult.Success)
                return;

            var inserted = DbFactory.MegrendelesCollection.Find(m => m.Statusz == megrendelesToInsert.Statusz).SingleOrDefault();
            if (areEqual(inserted, megrendelesToInsert) && inserted.MegrendelesTetelek.Length == 1 && inserted.MegrendelesTetelek[0].TermekID == termek1.ID
                && inserted.MegrendelesTetelek[0].Mennyiseg == mennyiseg && inserted.MegrendelesTetelek[0].NettoAr == termekToInsert.NettoAr
                && inserted.MegrendelesTetelek[0].Statusz == megrendelesToInsert.Statusz)
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} ok");
            }
            else
            {
                ahkResult.AddProblem($"{operationName} nem szurta be a megadott termeket. {operationName} did not insert the provided product");
            }
        }

        private static void testUpdate(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.UpdateMegrendeles)}";

            var updatedMegrendeles = new MongoLabor.Models.Megrendeles
            {
                ID = megrendeles.ID.ToString(),
                Statusz = megrendeles.Statusz,
                FizetesMod = Guid.NewGuid().ToString(),
                Datum = megrendeles.Datum,
                Hatarido = megrendeles.Hatarido.Value.AddDays(4),
            };

            var successful = true;
            var repoUpdateResult = Op.Func(() => repository.UpdateMegrendeles(updatedMegrendeles)).TryRunOperation(ahkResult, operationName);
            if (repoUpdateResult.Success && repoUpdateResult.Value && areEqual(DbFactory.MegrendelesCollection.Find(m => m.ID == megrendeles.ID).SingleOrDefault(), updatedMegrendeles))
            {
                ahkResult.Log($"{operationName} - sikeres modositas ok. {operationName} - successful update ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} nem vegezte el a modositast. {operationName} did not execute the update");
            }

            updatedMegrendeles.ID = ObjectId.Empty.ToString();
            repoUpdateResult = Op.Func(() => repository.UpdateMegrendeles(updatedMegrendeles)).TryRunOperation(ahkResult, operationName);
            if (repoUpdateResult.Success && !repoUpdateResult.Value)
            {
                ahkResult.Log($"{operationName} - megrendeles nem letezik ok. {operationName} - order does not exist ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} modositott nem letezo megrendelest. {operationName} updated a non-existent order");
            }

            if (successful)
                ahkResult.AddPoints(1);
        }

        private static void testDelete(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.DeleteMegrendeles)}";

            var repoDeleteResult = Op.Func(() => repository.DeleteMegrendeles(megrendeles.ID.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoDeleteResult.Success && !DbFactory.MegrendelesCollection.Find(m => m.ID == megrendeles.ID).Any())
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} ok");
            }
            else
            {
                ahkResult.AddProblem($"{operationName} nem torolte ki a megadott megrendelest. {operationName} did not delete the provided order");
            }
        }

        private static bool areEqual(Megrendeles a, MongoLabor.Models.Megrendeles b)
        {
            if (a == null && b == null)
                return true;
            else if (a == null || b == null)
                return false;
            else
                return b.Statusz == a.Statusz && b.Datum?.ToString("u") == a.Datum?.ToString("u") && b.Hatarido?.ToString("u") == a.Hatarido?.ToString("u")
                    && b.FizetesMod == a.FizetesMod.Mod;
        }
    }
}
