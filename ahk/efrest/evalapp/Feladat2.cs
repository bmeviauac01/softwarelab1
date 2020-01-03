using adatvez.Helpers;
using ahk.common;
using System;
using System.Threading.Tasks;

namespace adatvez
{
    internal class Feladat2
    {
        public const string AhkExerciseName = @"Feladat 2 - Exercise 2";

        private static readonly string TestTaskRecordTitle = Guid.NewGuid().ToString();
        private static int TestTaskRecordId;

        public static async Task Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 2 ###### Exercise 2 ######");

            try
            {
                if (prepareDb(ahkResult)) // minimum requirement to start evaluation is that the DbContext works
                {
                    await testRESTGetAll(ahkResult);
                    await testRESTPost(ahkResult);
                    await testRESTGet(ahkResult);
                    await testRESTDelete(ahkResult);

                    // screenshot is mandatory
                    if (!ScreenshotValidator.IsScreenshotPresent("f2.png", "f2.png", ahkResult))
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
            using (var scope = WebAppInit.GetRequestScope())
            {
                try
                {
                    // init database
                    var dbContext = DbHelper.GetDbContext(scope);
                    dbContext.Database.EnsureCreated();

                    // get primary key of EF entity
                    var findPrimaryKey = dbContext.TryGetPrimaryKey<api.DAL.EfDbContext.DbTask>(ahkResult);
                    if (!findPrimaryKey.Success)
                        return false;

                    ahkResult.Log("DbTask PK ok");

                    // check DbTask -> DbStatus foreign key connection
                    var findForeignKey = dbContext.TryGetForeignKey<api.DAL.EfDbContext.DbTask, api.DAL.EfDbContext.DbStatus>(ahkResult);
                    if (!findForeignKey.Success)
                        return false;

                    // check DbTask -> DbStatus navigation property
                    var findNavigationPropery = dbContext.TryGetNavigationPropery<api.DAL.EfDbContext.DbTask, api.DAL.EfDbContext.DbStatus>(ahkResult);
                    if (!findNavigationPropery.Success)
                        return false;

                    ahkResult.Log("DbTask -> DbStatus FK ok");

                    // add test data to database directly
                    var addRecordResult = dbContext.TryAddTaskRecord(TestTaskRecordTitle, findPrimaryKey.Value, findNavigationPropery.Value, ahkResult);
                    if (!addRecordResult.Success)
                        return false;

                    TestTaskRecordId = addRecordResult.Value;

                    ahkResult.Log("DbTask insert ok");
                    ahkResult.AddPoints(2);
                    return true;
                }
                catch (Exception ex)
                {
                    ahkResult.AddProblem(ex, "DbContext inicializalas / DbStatus rekord mentese sikertelen. Failed to initialize DbContext / failed to add DbStatus to database.");
                    return false;
                }
            }
        }

        /// <summary>
        /// GET /api/tasks
        /// </summary>
        private static async Task testRESTGetAll(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var restListResult = await scope.HttpClient.TryGet<api.Model.Task[]>("/api/tasks", ahkResult)
                                    .TryFindItem(s => s.Title.Equals(TestTaskRecordTitle, StringComparison.OrdinalIgnoreCase), ahkResult, "GET /api/tasks");
                if (restListResult.Success)
                {
                    ahkResult.AddPoints(1);
                    ahkResult.Log("GET /api/tasks ok");
                }
            }
        }

        /// <summary>
        /// POST /api/tasks
        /// </summary>
        private static async Task testRESTPost(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var newTitleForPost = Guid.NewGuid().ToString();
                var newStatusForPost = Guid.NewGuid().ToString();
                var postResponse = await scope.HttpClient.TryPostWithReturnValue<api.Model.Task>("/api/tasks", new api.Controllers.Dto.CreateTask() { Title = newTitleForPost, Status = newStatusForPost }, ahkResult, requireLocationHeader: true);
                if (postResponse.Success)
                {
                    var dbContext = scope.GetDbContext();
                    var postInsertedRecordFromDbContext = dbContext.GetTasksDbSet().Find(postResponse.Value.Id);
                    if (postInsertedRecordFromDbContext == null)
                    {
                        ahkResult.AddProblem("POST /api/tasks nem szurt be adatbazisba rekordot. POST /api/tasks did not save record into database.");
                    }
                    else
                    {
                        ahkResult.AddPoints(1);
                        ahkResult.Log("POST /api/tasks ok");
                    }
                }
            }
        }

        /// <summary>
        /// GET /api/tasks/id
        /// </summary>
        private static async Task testRESTGet(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                int randomint = DateTime.UtcNow.Millisecond * 897;

                var getExistingResponse = await scope.HttpClient.TryGet<api.Model.Task>($"/api/tasks/{TestTaskRecordId}", ahkResult);
                var getNotFoundResponse = await scope.HttpClient.TryGet<api.Model.Task>($"/api/tasks/{randomint}", ahkResult, allowNotFound: true);
                if (getExistingResponse.Success && getNotFoundResponse.Success)
                {
                    if (getExistingResponse.Value.Title.Equals(TestTaskRecordTitle, StringComparison.OrdinalIgnoreCase) && getNotFoundResponse.Value == null)
                    {
                        ahkResult.AddPoints(1);
                        ahkResult.Log("GET /api/tasks/id ok");
                    }
                    else
                    {
                        ahkResult.AddProblem("GET /api/tasks/id valasz tartalma hibas. GET /api/tasks/id yields invalid content.");
                    }
                }
            }
        }

        /// <summary>
        /// DELETE /api/tasks/id
        /// </summary>
        private static async Task testRESTDelete(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var deleteOkResponse = await scope.HttpClient.TryDelete($"/api/tasks/{TestTaskRecordId}", ahkResult);
                var deleteNotFoundResponse = await scope.HttpClient.TryDelete($"/api/tasks/{TestTaskRecordId}", ahkResult);
                if (deleteOkResponse.Success && deleteNotFoundResponse.Success)
                {
                    if (deleteOkResponse.Value == true && deleteNotFoundResponse.Value == false)
                    {
                        ahkResult.AddPoints(1);
                        ahkResult.Log("DELETE /api/tasks/id ok");
                    }
                    else
                    {
                        ahkResult.AddProblem("DELETE /api/tasks/id valasz tartalma hibas. DELETE /api/tasks/id yields invalid reponse.");
                    }
                }
            }
        }
    }
}
