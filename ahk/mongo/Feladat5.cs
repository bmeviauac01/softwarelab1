using adatvez.DAL;
using ahk.common;
using ahk.common.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoLabor.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace adatvez
{
    internal static class Feladat5
    {
        public const string AhkExerciseName = @"imsc@Feladat 5 - Exercise 5";

        private static IEqualityComparer<DateTime> dateTimeComparer = new DateTimeComparer(leeway: TimeSpan.FromDays(1));

        private static readonly Termek[] termekek = new[]
        {
            RandomEntityFactory.CreateRandomTermek(),
            RandomEntityFactory.CreateRandomTermek(),
        };

        private static readonly int csoportCount = RandomHelper.GetRandomValue(7, 15);

        private static Megrendeles firstMegrendeles;
        private static Megrendeles lastMegrendeles;

        private static DateTime csoportStart;
        private static Megrendeles[] megrendelesek;

        private static int hatarokSuccesses = 0;
        private static int csoportokSuccesses = 0;

        public static void Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 5 ###### Exercise 5 ######");

            try
            {
                if (prepareDb(ahkResult))
                {
                    var repository = new MongoLabor.DAL.AdatvezRepository(DbFactory.Database);

                    testOneCsoport(repository, ahkResult);
                    testMultipleCsoport(repository, ahkResult);

                    if (hatarokSuccesses == 2)
                        ahkResult.AddPoints(1);
                    if (csoportokSuccesses == 2)
                        ahkResult.AddPoints(2);

                    if (!ScreenshotValidator.IsScreenshotPresent("f5-megrendelescsoportok.png", "f5-megrendelescsoportok.png", ahkResult))
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
                DbFactory.MegrendelesCollection.DeleteMany(Builders<Megrendeles>.Filter.Empty);

                var vevoId = ObjectId.Parse("5d7e42adcffa8e1b64f7dbb9");
                var telephelyId = ObjectId.Parse("5d7e42adcffa8e1b64f7dbba");
                var firstDate = DateTime.UtcNow.AddYears(RandomHelper.GetRandomValue(-14, -7));
                var lastDate = DateTime.UtcNow.AddYears(RandomHelper.GetRandomValue(8, 16));

                firstMegrendeles = RandomEntityFactory.CreateRandomMegrendeles(vevoId, telephelyId, firstDate, termekek);
                lastMegrendeles = RandomEntityFactory.CreateRandomMegrendeles(vevoId, telephelyId, lastDate, termekek);
                DbFactory.MegrendelesCollection.InsertOne(firstMegrendeles);
                DbFactory.MegrendelesCollection.InsertOne(lastMegrendeles);

                var interval = (lastDate - firstDate) / csoportCount;
                csoportStart = (firstDate + interval * 3);
                var csoportEnd = (firstDate + interval * 4);

                int count = RandomHelper.GetRandomValue(10, 20);
                megrendelesek = Enumerable.Range(0, count)
                    .Select(_ => RandomEntityFactory.CreateRandomMegrendeles(vevoId, telephelyId, RandomHelper.GetRandomValue(csoportStart.AddMonths(1), csoportEnd.AddMonths(-1)), termekek))
                    .ToArray();
                DbFactory.MegrendelesCollection.InsertMany(megrendelesek);

                ahkResult.Log("Termek and Megrendeles insert ok");
                return true;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "Db inicializalas / Termek es Megrendeles mentese sikertelen. Failed to initialize Db / failed to add Product and Order to database.");
                return false;
            }
        }

        private static void testOneCsoport(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.MegrendelesCsoportosit)} - 1";

            var repoCsoportositResult = Op.Func(() => repository.MegrendelesCsoportosit(1)).TryRunOperation(ahkResult, operationName);
            if (!repoCsoportositResult.Success)
                return;

            testHatarok(repoCsoportositResult.Value.Hatarok, 1, ahkResult, operationName);

            var csoportokResult = repoCsoportositResult.Value.Csoportok.ToArray().TryCheckLength(1, ahkResult, operationName);
            if (!csoportokResult.Success)
                return;

            var itemResult = csoportokResult.TryFindItem(cs => dateTimeComparer.Equals(cs.Datum, firstMegrendeles.Datum.Value) && cs.Darab == megrendelesek.Length + 2, ahkResult, operationName);
            if (itemResult.Success)
            {
                ahkResult.Log($"{operationName} ok");
                csoportokSuccesses++;
            }
        }

        private static void testMultipleCsoport(MongoLabor.DAL.AdatvezRepository repository, AhkResult ahkResult)
        {
            string operationName = $"{nameof(MongoLabor.DAL.IAdatvezRepository)}.{nameof(MongoLabor.DAL.IAdatvezRepository.MegrendelesCsoportosit)} - (tobb - multiple)";

            var repoCsoportositResult = Op.Func(() => repository.MegrendelesCsoportosit(csoportCount)).TryRunOperation(ahkResult, operationName);
            if (!repoCsoportositResult.Success)
                return;

            testHatarok(repoCsoportositResult.Value.Hatarok, csoportCount, ahkResult, operationName);

            if (repoCsoportositResult.Value.Csoportok.Count < 3 || repoCsoportositResult.Value.Csoportok.Count > csoportCount)
            {
                ahkResult.AddProblem($"{operationName} nem a megfelelo mennyisegu elemet adja vissza. {operationName} does not return the proper amount of items");
                return;
            }

            var itemResult = repoCsoportositResult.Value.Csoportok.TryFindItem(
                cs => dateTimeComparer.Equals(cs.Datum, csoportStart) && cs.Darab == megrendelesek.Length
                && cs.OsszErtek == megrendelesek.Sum(m => m.MegrendelesTetelek.Sum(mt => mt.Mennyiseg * mt.NettoAr)), ahkResult, operationName);
            if (itemResult.Success)
            {
                ahkResult.Log($"{operationName} - csoportok ok. {operationName} - groups ok");
                csoportokSuccesses++;
            }
        }

        private static void testHatarok(IList<DateTime> hatarok, int csoportDarab, AhkResult ahkResult, string operationName)
        {
            var max = lastMegrendeles.Datum.Value.AddHours(1);

            var intervallum = (max - firstMegrendeles.Datum.Value) / csoportDarab;
            var expectedHatarok = new List<DateTime>(capacity: csoportDarab + 1);
            for (int i = 0; i < csoportDarab; i++)
                expectedHatarok.Add(firstMegrendeles.Datum.Value + intervallum * i);
            expectedHatarok.Add(max);

            if (expectedHatarok.SequenceEqual(hatarok, dateTimeComparer))
            {
                hatarokSuccesses++;
                ahkResult.Log($"{operationName} - hatarok ok. {operationName} - boundaries ok");
            }
            else
            {
                ahkResult.AddProblem($"{operationName} nem a megfelelo hatarokat allitotta elo. {operationName} did not produce the expected boundaries");
            }
        }
    }
}
