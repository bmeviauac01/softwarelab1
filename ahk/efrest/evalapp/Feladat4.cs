using adatvez.Helpers;
using ahk.common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adatvez
{
    internal class Feladat4
    {
        public const string AhkExerciseName = @"imsc@Feladat 4 - Exercise 4";

        public static async Task Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 4 ###### Exercise 4 ######");

            try
            {
                if (await createTestTasks(ahkResult, 15)) // minimum requirement to start evaluation is that a large number of task can be created through the API
                {
                    await testPaged(ahkResult, perPage: 10, countShouldBeAtLeast: 15);

                    // screenshot is mandatory
                    if (!ScreenshotValidator.IsScreenshotPresent("f4.png", "f4.png", ahkResult))
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

        /// <summary>
        /// POST /api/tasks
        /// </summary>
        private static async Task<bool> createTestTasks(AhkResult ahkResult, int count)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var newStatusForPost = Guid.NewGuid().ToString();
                for (int i = 0; i < count; ++i)
                {
                    var newTitleForPost = Guid.NewGuid().ToString();
                    var postResponse = await scope.HttpClient.TryPostWithReturnValue<api.Model.Task>("/api/tasks", new api.Controllers.Dto.CreateTask() { Title = newTitleForPost, Status = newStatusForPost }, ahkResult, requireLocationHeader: true);
                    if (!postResponse.Success)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// GET /api/tasks/paged
        /// </summary>
        private static async Task testPaged(AhkResult ahkResult, int perPage, int countShouldBeAtLeast)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                int maxAllowedCalls = (countShouldBeAtLeast / perPage) * 5;
                int itemCount = 0;
                var uniqueItemIds = new HashSet<int>();

                string nextUrl = $"/api/tasks/paged?count={perPage}";
                while (nextUrl != null && --maxAllowedCalls > 0)
                {
                    var response = await scope.HttpClient.TryGet<api.Controllers.Dto.PagedTaskList>(nextUrl, ahkResult);
                    if (response.Success)
                    {
                        nextUrl = response.Value.NextUrl;
                        if (response.Value.Items != null)
                        {
                            itemCount += response.Value.Items.Count;

                            foreach (var i in response.Value.Items)
                            {
                                if (!uniqueItemIds.Add(i.Id))
                                {
                                    ahkResult.AddProblem("Egy task tobbszor kerul listazasra. A task is returned more than once.");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        ahkResult.AddProblem("Lapozasos listazas hibas valasz. Paged listing responds with invalid value.");
                        return;
                    }
                }

                if (maxAllowedCalls == 0)
                {
                    ahkResult.AddProblem("Lapozasos listazas tobb lapot ad, mint amennyi elem van. Paged listing returns more pages than actual items.");
                    return;
                }

                if (itemCount < countShouldBeAtLeast)
                {
                    ahkResult.AddProblem("Lapozas nem adot vissza minden elemet. Paged listing did not return all items.");
                    return;
                }

                ahkResult.AddPoints(3);
                ahkResult.Log("GET /api/tasks/paged ok");
            }
        }
    }
}
