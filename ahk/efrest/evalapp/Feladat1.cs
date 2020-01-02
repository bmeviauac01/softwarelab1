using ahk.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace adatvez
{
    internal class Feladat1
    {
        public const string AhkExerciseName = @"Feladat 1";

        public static async Task Execute(AhkResult result)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 1 ######");

            try
            {
                await test1(result);
                await test2(result);
            }
            catch (MissingMethodException ex)
            {
                result.AddProblem(ex, "Nem megengedett kodot valtoztattal. Changed code that you should not have.");
            }
            catch (TypeLoadException ex)
            {
                result.AddProblem(ex, "Nem megengedett kodot valtoztattal. Changed code that you should not have.");
            }
        }

        private static async Task test1(AhkResult result)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                // init database
                var dbContext = DbHelper.GetDbContext(scope);
                dbContext.Database.EnsureCreated();

                // add test data to database directly
                var newIds = new List<int>();
                try
                {
                    for (int i = 0; i < 5; ++i)
                        newIds.Add(dbContext.AddStatusRecord($"stat {DateTime.UtcNow.Millisecond}{i}"));
                }
                catch (Exception ex)
                {
                    result.AddProblem(ex, "DbStatus rekordok adatbazisba mentese sikertelen. Cannot add DbStatus records to the database.");
                    return;
                }

                // List through repository
                {
                    IReadOnlyCollection<api.Model.Status> statusesFromRepository = null;
                    try
                    {
                        var repo = DbHelper.GetStatusesRepository(scope);
                        statusesFromRepository = repo.List();
                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "IStatusesRepository.List hibat dob. IStatusesRepository.List throws error.");
                        return;
                    }

                    if (statusesFromRepository == null)
                    {
                        result.AddProblem("IStatusesRepository.List null-lal ter vissza. IStatusesRepository.List yields null value.");
                        return;
                    }

                    if (statusesFromRepository.Count < 5)
                    {
                        result.AddProblem("IStatusesRepository.List nem ad vissza az adatbazisban letezo rekordot. IStatusesRepository.List does not return a record from the database.");
                        return;
                    }

                    var specificRecordFromRepository = statusesFromRepository.FirstOrDefault(s => s.Name.StartsWith("stat", StringComparison.OrdinalIgnoreCase) && newIds.Contains(s.Id));
                    if (specificRecordFromRepository == null)
                    {
                        result.AddProblem("IStatusesRepository.List nem ad vissza az adatbazisban letezo rekordot. IStatusesRepository.List does not return a record from the database.");
                        return;
                    }

                    result.AddPoints(2);
                    result.Log("IStatusesRepository.List ok");
                }

                // list through REST
                {
                    var httpResponse = await scope.HttpClient.GetAsync("/api/statuses");
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        result.AddProblem($"GET /api/statuses hibas valaszkod {httpResponse.StatusCode}. GET /api/statuses yields invalid response {httpResponse.StatusCode}.");
                        return;
                    }

                    api.Model.Status[] statusesFromController;
                    try
                    {
                        statusesFromController = await httpResponse.Content.ReadAsAsync<api.Model.Status[]>();
                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "GET /api/statuses valasz tartalma hibas. GET/api/statuses yields invalid content.");
                        return;
                    }

                    if (statusesFromController.Length < 5)
                    {
                        result.AddProblem("GET /api/statuses nem ad vissza az adatbazisban letezo rekordot. GET /api/statuses does not return a record from the database.");
                        return;
                    }

                    var specificRecordFromController = statusesFromController.FirstOrDefault(s => s.Name.StartsWith("stat", StringComparison.OrdinalIgnoreCase) && newIds.Contains(s.Id));
                    if (specificRecordFromController == null)
                    {
                        result.AddProblem("GET /api/statuses nem ad vissza az adatbazisban letezo rekordot. GET /api/statuses does not return a record from the database.");
                        return;
                    }

                    result.AddPoints(2);
                    result.Log("GET /api/statuses ok");
                }
            }
        }

        private static async Task test2(AhkResult result)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                // insert using repository
                {
                    var dbContext = scope.GetDbContext();
                    bool insertOk = true;

                    try
                    {
                        var repo = DbHelper.GetStatusesRepository(scope);

                        var newStatusName = Guid.NewGuid().ToString();
                        var insertedRecord = repo.Insert(new api.Controllers.Dto.CreateStatus() { Name = newStatusName });

                        if (insertedRecord == null || insertedRecord.Name != newStatusName)
                        {
                            insertOk = false;
                            result.AddProblem("IStatusesRepository.Insert visszateresi erteke hibas. IStatusesRepository.Insert has invalid return value.");
                        }

                        var insertedRecordFromDbContext = dbContext.GetStatusesDbSet().Find(insertedRecord.Id);
                        if (insertedRecordFromDbContext == null || insertedRecordFromDbContext.ReadStatusRecordName() != newStatusName)
                        {
                            insertOk = false;
                            result.AddProblem("IStatusesRepository.Insert nem szurt be rekordot adatbazisba. IStatusesRepository.Insert failed to insert data into database.");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "IStatusesRepository.Insert hibat dob. IStatusesRepository.Insert throws error.");
                    }

                    if (insertOk)
                    {
                        result.Log("IStatusesRepository.Insert ok");
                        result.AddPoints(1);
                    }
                }

                // exists/get using repository
                {
                    var dbContext = scope.GetDbContext();
                    bool existsOk = true;
                    bool findOk = true;

                    try
                    {
                        var repo = DbHelper.GetStatusesRepository(scope);

                        var newStatusName = Guid.NewGuid().ToString();
                        var insertedRecordId = dbContext.AddStatusRecord(newStatusName);

                        if (!repo.ExistsWithName(newStatusName))
                        {
                            existsOk = false;
                            result.AddProblem("IStatusesRepository.ExistsWithName rossz valasz. IStatusesRepository.ExistsWithName returns wrong result.");
                        }

                        if (repo.ExistsWithName(Guid.NewGuid().ToString()))
                        {
                            existsOk = false;
                            result.AddProblem("IStatusesRepository.ExistsWithName rossz valasz. IStatusesRepository.ExistsWithName returns wrong result.");
                        }

                        var findByNotExists = repo.FindById(458973459);
                        if (findByNotExists != null)
                        {
                            findOk = false;
                            result.AddProblem("IStatusesRepository.FindById rossz valasz. IStatusesRepository.FindById returns wrong result.");
                        }

                        var findByExists = repo.FindById(insertedRecordId);
                        if (findByExists == null || findByExists.Name != newStatusName)
                        {
                            findOk = false;
                            result.AddProblem("IStatusesRepository.FindById rossz valasz. IStatusesRepository.FindById returns wrong result.");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "IStatusesRepository.FindById/ExistsWithName hibat dob. IStatusesRepository.ExistsWithName throws error.");
                    }

                    if (existsOk && findOk)
                    {
                        result.Log("IStatusesRepository.FindById & ExistsWithName ok");
                        result.AddPoints(1);
                    }
                }

                // insert through REST
                {
                    var newStatusName = Guid.NewGuid().ToString();
                    bool postOk = true;

                    var httpResponse = await scope.HttpClient.PostAsJsonAsync("/api/statuses", new api.Controllers.Dto.CreateStatus() { Name = newStatusName });
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        postOk = false;
                        result.AddProblem($"POST /api/statuses hibas valaszkod {httpResponse.StatusCode}. POST /api/statuses yields invalid response {httpResponse.StatusCode}.");
                    }
                    if (!httpResponse.Headers.Contains(Microsoft.Net.Http.Headers.HeaderNames.Location) || httpResponse.Headers.Location == null)
                    {
                        postOk = false;
                        result.AddProblem("POST /api/statuses valaszban hianyzo header. POST /api/statuses reponse missing header.");
                    }

                    api.Model.Status postResponseAsObject = null;
                    try
                    {
                        postResponseAsObject = await httpResponse.Content.ReadAsAsync<api.Model.Status>();
                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "POST /api/statuses valasz tartalma hibas. POST /api/statuses yields invalid content.");
                        postOk = false;
                    }

                    if (postResponseAsObject != null)
                    {
                        var dbContext = scope.GetDbContext();
                        var postInsertedRecordFromDbContext = dbContext.GetStatusesDbSet().Find(postResponseAsObject.Id);

                        if (postInsertedRecordFromDbContext == null || postInsertedRecordFromDbContext.ReadStatusRecordName() != newStatusName)
                        {
                            result.AddProblem("POST /api/statuses nem szurt be adatbazisba rekordot. POST /api/statuses did not save record into database.");
                            postOk = false;
                        }
                    }

                    if (postOk)
                    {
                        result.AddPoints(1);
                        result.Log("POST /api/statuses ok");
                    }
                }

                // exists/get using REST
                {
                    var dbContext = scope.GetDbContext();
                    bool headOk = true;
                    bool getOk = true;

                    try
                    {
                        var repo = DbHelper.GetStatusesRepository(scope);

                        var newStatusName = Guid.NewGuid().ToString();
                        var insertedRecordId = dbContext.AddStatusRecord(newStatusName);

                        var httpResponseHead = await scope.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, $"/api/statuses/{newStatusName}"));
                        if (httpResponseHead.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            headOk = false;
                            result.AddProblem($"HEAD /api/statuses/name hibas valaszkod {httpResponseHead.StatusCode}. HEAD /api/statuses/name yields invalid response {httpResponseHead.StatusCode}.");
                        }

                        var httpResponseGet = await scope.HttpClient.GetAsync($"/api/statuses/{insertedRecordId}");
                        if (httpResponseGet.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            getOk = false;
                            result.AddProblem($"GET /api/statuses/id hibas valaszkod {httpResponseGet.StatusCode}. GET /api/statuses/id yields invalid response {httpResponseGet.StatusCode}.");
                        }

                        api.Model.Status getResponseAsObject = null;
                        try
                        {
                            getResponseAsObject = await httpResponseGet.Content.ReadAsAsync<api.Model.Status>();
                        }
                        catch (Exception ex)
                        {
                            result.AddProblem(ex, "GET /api/statuses/id valasz tartalma hibas. POST /api/statuses/id yields invalid content.");
                            getOk = false;
                        }

                        if (getResponseAsObject.Name != newStatusName || getResponseAsObject.Id != insertedRecordId)
                        {
                            result.AddProblem("GET /api/statuses/id valasz tartalma hibas. POST /api/statuses/id yields invalid content.");
                            getOk = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        result.AddProblem(ex, "IStatusesRepository.FindById/ExistsWithName hibat dob. IStatusesRepository.ExistsWithName throws error.");
                    }

                    if (headOk && getOk)
                    {
                        result.Log("IStatusesRepository.FindById & ExistsWithName ok");
                        result.AddPoints(1);
                    }
                }
            }
        }
    }
}
