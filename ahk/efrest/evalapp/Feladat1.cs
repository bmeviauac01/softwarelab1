using adatvez.Helpers;
using ahk.common;
using System;
using System.Threading.Tasks;

namespace adatvez
{
    internal class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1";

        private static readonly string TestStatusRecordName = Guid.NewGuid().ToString();
        private static int TestStatusRecordId;

        public static async Task Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 1 ######");

            try
            {
                if (prepareDb(ahkResult)) // minimum requirement to start evaluation is that the DbContext works
                {
                    testRepoList(ahkResult);
                    await testRESTGetAll(ahkResult);
                    testRepoInsert(ahkResult);
                    testRepoGetExist(ahkResult);
                    await testRESTPost(ahkResult);
                    await testRESTHeadGet(ahkResult);
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
            using (var scope = WebAppInit.GetRequestScope())
            {
                try
                {
                    // init database
                    var dbContext = DbHelper.GetDbContext(scope);
                    dbContext.Database.EnsureCreated();

                    // add test data to database directly
                    var addRecordResult = dbContext.TryAddStatusRecord(TestStatusRecordName, ahkResult);
                    if (addRecordResult.Success)
                        TestStatusRecordId = addRecordResult.Value;

                    return addRecordResult.Success;
                }
                catch (Exception ex)
                {
                    ahkResult.AddProblem(ex, "DbContext inicializalas / DbStatus rekord mentese sikertelen. Failed to initialize DbContext / failed to add DbStatus to database.");
                    return false;
                }
            }
        }

        /// <summary>
        /// IStatusesRepository.List
        /// </summary>
        private static void testRepoList(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var repoListResult = Op.Func(() => scope.GetStatusesRepository().List())
                                     .TryRunOperationAndFindItem(s => s.Name.Equals(TestStatusRecordName, StringComparison.OrdinalIgnoreCase), ahkResult, "IStatusesRepository.List");
                if (repoListResult.Success)
                {
                    ahkResult.AddPoints(2);
                    ahkResult.Log("IStatusesRepository.List ok");
                }
            }
        }

        /// <summary>
        /// GET /api/statuses
        /// </summary>
        private static async Task testRESTGetAll(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var restListResult = await scope.HttpClient.TryGet<api.Model.Status[]>("/api/statuses", ahkResult)
                                    .TryFindItem(s => s.Name.Equals(TestStatusRecordName, StringComparison.OrdinalIgnoreCase), ahkResult, "GET /api/statuses");
                if (restListResult.Success)
                {
                    ahkResult.AddPoints(2);
                    ahkResult.Log("GET /api/statuses ok");
                }
            }
        }

        /// <summary>
        /// IStatusesRepository.Insert
        /// </summary>
        private static void testRepoInsert(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var dbContext = scope.GetDbContext();
                var newStatusName = Guid.NewGuid().ToString();

                var repoInsertResult = Op.Func(() => scope.GetStatusesRepository().Insert(new api.Controllers.Dto.CreateStatus() { Name = newStatusName }))
                                       .TryRunOperation(ahkResult, "IStatusesRepository.Insert");
                if (repoInsertResult.Success)
                {
                    if (repoInsertResult.Value == null || !repoInsertResult.Value.Name.Equals(newStatusName, StringComparison.OrdinalIgnoreCase))
                    {
                        ahkResult.AddProblem("IStatusesRepository.Insert visszateresi erteke hibas. IStatusesRepository.Insert has invalid return value.");
                    }
                    else
                    {
                        var insertedRecordFromDbContext = dbContext.GetStatusesDbSet().Find(repoInsertResult.Value.Id);
                        if (insertedRecordFromDbContext == null || !insertedRecordFromDbContext.ReadStatusRecordName(ahkResult).Equals(newStatusName, StringComparison.OrdinalIgnoreCase))
                        {
                            ahkResult.AddProblem("IStatusesRepository.Insert nem szurt be rekordot adatbazisba. IStatusesRepository.Insert failed to insert data into database.");
                        }
                        else
                        {
                            ahkResult.Log("IStatusesRepository.Insert ok");
                            ahkResult.AddPoints(1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// IStatusesRepository.FindById
        /// IStatusesRepository.ExistsWithName
        /// </summary>
        private static void testRepoGetExist(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var dbContext = scope.GetDbContext();
                bool existsOk = false;
                bool findOk = false;

                var existsResult1 = Op.Func(() => scope.GetStatusesRepository().ExistsWithName(TestStatusRecordName))
                                    .TryRunOperation(ahkResult, "IStatusesRepository.ExistsWithName");
                var existsResult2 = Op.Func(() => scope.GetStatusesRepository().ExistsWithName(Guid.NewGuid().ToString()))
                                    .TryRunOperation(ahkResult, "IStatusesRepository.ExistsWithName");
                if (existsResult1.Success && existsResult2.Success)
                {
                    if (existsResult1.Value == true && existsResult2.Value == false)
                        existsOk = true;
                    else
                        ahkResult.AddProblem("IStatusesRepository.ExistsWithName rossz valasz. IStatusesRepository.ExistsWithName returns wrong result.");
                }

                var findResult1 = Op.Func(() => scope.GetStatusesRepository().FindById(TestStatusRecordId))
                                    .TryRunOperation(ahkResult, "IStatusesRepository.FindById");
                var findResult2 = Op.Func(() => scope.GetStatusesRepository().FindById(458973459))
                                    .TryRunOperation(ahkResult, "IStatusesRepository.FindById");

                if (findResult1.Success && findResult2.Success)
                {
                    if (findResult1.Value != null && findResult1.Value.Name.Equals(TestStatusRecordName, StringComparison.OrdinalIgnoreCase)
                        && findResult2.Value == null)
                        findOk = true;
                    else
                        ahkResult.AddProblem("IStatusesRepository.FindById rossz valasz. IStatusesRepository.ExistsWithName returns wrong result.");
                }

                if (existsOk && findOk)
                {
                    ahkResult.Log("IStatusesRepository.FindById & ExistsWithName ok");
                    ahkResult.AddPoints(1);
                }
            }
        }

        /// <summary>
        /// POST /api/statuses
        /// </summary>
        private static async Task testRESTPost(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var dbContext = scope.GetDbContext();

                // insert through REST
                var newStatusNameForPost = Guid.NewGuid().ToString();
                var postResponse = await scope.HttpClient.TryPostWithReturnValue<api.Model.Status>("/api/statuses", new api.Controllers.Dto.CreateStatus() { Name = newStatusNameForPost }, ahkResult, requireLocationHeader: true);
                if (postResponse.Success)
                {
                    var postInsertedRecordFromDbContext = dbContext.GetStatusesDbSet().Find(postResponse.Value.Id);
                    if (postInsertedRecordFromDbContext == null || postInsertedRecordFromDbContext.ReadStatusRecordName(ahkResult) != newStatusNameForPost)
                    {
                        ahkResult.AddProblem("POST /api/statuses nem szurt be adatbazisba rekordot. POST /api/statuses did not save record into database.");
                    }
                    else
                    {
                        ahkResult.AddPoints(1);
                        ahkResult.Log("POST /api/statuses ok");
                    }
                }
            }
        }

        /// <summary>
        /// HEAD /api/statuses/name
        /// GET /api/statuses/id
        /// </summary>
        private static async Task testRESTHeadGet(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                // exists/get using REST
                bool headOk = false;
                bool getOk = false;

                var httpResponseHead = await scope.HttpClient.TryHead($"/api/statuses/{TestStatusRecordName}", ahkResult);
                if (httpResponseHead.Success)
                {
                    if (httpResponseHead.Value == true)
                        headOk = true;
                    else
                        ahkResult.AddProblem($"HEAD /api/statuses/name hibas valasz. HEAD /api/statuses/name yields invalid response.");
                }

                var getResponse = await scope.HttpClient.TryGet<api.Model.Status>($"/api/statuses/{TestStatusRecordId}", ahkResult);
                if (getResponse.Success)
                {
                    if (getResponse.Value.Name.Equals(TestStatusRecordName, StringComparison.OrdinalIgnoreCase) && getResponse.Value.Id == TestStatusRecordId)
                        getOk = true;
                    else
                        ahkResult.AddProblem("GET /api/statuses/id valasz tartalma hibas. POST /api/statuses/id yields invalid content.");
                }

                if (headOk && getOk)
                {
                    ahkResult.Log("GET /api/statuses/id && HEAD /api/statuses/name ok");
                    ahkResult.AddPoints(1);
                }
            }
        }
    }
}
