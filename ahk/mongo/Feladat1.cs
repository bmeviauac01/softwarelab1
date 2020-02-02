using adatvez.DAL;
using adatvez.Helpers;
using ahk.common;
using ahk.common.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoLabor.DAL.Entities;
using System;
using System.Linq;

namespace adatvez
{
    internal static class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1 - Exercise 1";

        private static readonly Termek termek = RandomEntityFactory.CreateRandomTermek();

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 1 ###### Exercise 1 ######");

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

                    if (!ScreenshotValidator.IsScreenshotPresent("f1-termekek.png", "f1-termekek.png", ahkResult))
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
                DbFactory.TermekCollection.InsertOne(termek);
                ahkResult.Log("Termek insert ok");
                return true;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "Db inicializalas / Termek mentese sikertelen. Failed to initialize Db / failed to add Product to database.");
                return false;
            }
        }

        private static void testList(MongoLabor.DAL.IAdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.ListTermekek)}";

            var repoListResult = Op.Func(() => repository.ListTermekek().ToArray()).TryRunOperationAndCheckLength(11, ahkResult, operationName);
            if (!repoListResult.Success)
                return;

            var repoItemResult = repoListResult.TryFindItem(t => t.ID == termek.ID.ToString() && t.Nev == termek.Nev, ahkResult, operationName);
            if (repoItemResult.Success)
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} ok");
            }
        }

        private static void testFind(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.FindTermek)}";

            var successful = true;
            var repoFindResult = Op.Func(() => repository.FindTermek(termek.ID.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoFindResult.Success && repoFindResult.Value != null && repoFindResult.Value.ID == termek.ID.ToString() && repoFindResult.Value.Nev == termek.Nev)
            {
                ahkResult.Log($"{operationName} - letezo termek ok. {operationName} - existing product ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} nem az elvart ertekkel ter vissza. {operationName} does not return the expected result");
            }

            repoFindResult = Op.Func(() => repository.FindTermek(ObjectId.Empty.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoFindResult.Success && repoFindResult.Value == null)
            {
                ahkResult.Log($"{operationName} - nem letezo termek ok. {operationName} - non-existent product ok");
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
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.InsertTermek)}";

            var termekToInsert = new MongoLabor.Models.Termek
            {
                Nev = Guid.NewGuid().ToString(),
                NettoAr = 345.6,
                Raktarkeszlet = RandomHelper.GetRandomValue(15, 30),
            };

            var repoInsertResult = Op.Func(() => repository.InsertTermek(termekToInsert)).TryRunOperation(ahkResult, operationName);
            if (repoInsertResult.Success && DbFactory.TermekCollection.Find(t => t.Nev == termekToInsert.Nev).SingleOrDefault()?.Raktarkeszlet == termekToInsert.Raktarkeszlet)
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
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.TermekElad)}";

            var successful = true;
            var repoUpdateResult = Op.Func(() => repository.TermekElad(termek.ID.ToString(), 50)).TryRunOperation(ahkResult, operationName);
            if (repoUpdateResult.Success && repoUpdateResult.Value && DbFactory.TermekCollection.Find(t => t.ID == termek.ID).SingleOrDefault()?.Raktarkeszlet == 6)
            {
                ahkResult.Log($"{operationName} - sikeres modositas ok. {operationName} - successful update ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} nem vegezte el a modositast. {operationName} did not execute the update");
            }

            repoUpdateResult = Op.Func(() => repository.TermekElad(termek.ID.ToString(), 10)).TryRunOperation(ahkResult, operationName);
            if (repoUpdateResult.Success && !repoUpdateResult.Value && DbFactory.TermekCollection.Find(t => t.ID == termek.ID).SingleOrDefault()?.Raktarkeszlet == 6)
            {
                ahkResult.Log($"{operationName} - nincs eleg raktaron ok. {operationName} - not enough in stock ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} eladott nem rataron levo termeket. {operationName} sold products that were not in stock");
            }

            repoUpdateResult = Op.Func(() => repository.TermekElad(ObjectId.Empty.ToString(), 1)).TryRunOperation(ahkResult, operationName);
            if (repoUpdateResult.Success && !repoUpdateResult.Value)
            {
                ahkResult.Log($"{operationName} - termek nem letezik ok. {operationName} - product does not exist ok");
            }
            else
            {
                successful = false;
                ahkResult.AddProblem($"{operationName} eladott nem letezo termeket. {operationName} sold a non-existent product");
            }

            if (successful)
                ahkResult.AddPoints(3);
        }

        private static void testDelete(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.DeleteTermek)}";

            var repoDeleteResult = Op.Func(() => repository.DeleteTermek(termek.ID.ToString())).TryRunOperation(ahkResult, operationName);
            if (repoDeleteResult.Success && !DbFactory.TermekCollection.Find(t => t.ID == termek.ID).Any())
            {
                ahkResult.AddPoints(1);
                ahkResult.Log($"{operationName} ok");
            }
            else
            {
                ahkResult.AddProblem($"{operationName} nem torolte ki a megadott termeket. {operationName} did not delete the provided product");
            }
        }
    }
}
