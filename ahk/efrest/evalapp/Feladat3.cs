using adatvez.Helpers;
using ahk.common;
using System;
using System.Threading.Tasks;

namespace adatvez
{
    internal class Feladat3
    {
        public const string AhkExerciseName = @"Feladat 3 - Exercise 3";

        private static string TestTaskRecordTitle;
        private static int TestTaskRecordId;

        public static async Task Execute(AhkResult ahkResult)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("###### Feladat 3 ###### Exercise 3 ######");

            try
            {
                if (await createTestTask(ahkResult)) // minimum requirement to start evaluation is that a task can be created through the API
                {
                    await testMarkDone(ahkResult);
                    await testMoveToNewStatus(ahkResult);

                    // screenshot is mandatory
                    if (!ScreenshotValidator.IsScreenshotPresent("f3.png", "f3.png", ahkResult))
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
        private static async Task<bool> createTestTask(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                var newTitleForPost = Guid.NewGuid().ToString();
                var newStatusForPost = Guid.NewGuid().ToString();
                var postResponse = await scope.HttpClient.TryPostWithReturnValue<api.Model.Task>("/api/tasks", new api.Controllers.Dto.CreateTask() { Title = newTitleForPost, Status = newStatusForPost }, ahkResult, requireLocationHeader: true);
                if (postResponse.Success)
                {
                    TestTaskRecordId = postResponse.Value.Id;
                    TestTaskRecordTitle = postResponse.Value.Title;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// PATCH /api/tasks/{id}/done
        /// </summary>
        private static async Task testMarkDone(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                int randomint = DateTime.UtcNow.Millisecond * 897;

                var patchExistingResponse = await scope.HttpClient.TryPatch<api.Model.Task>($"/api/tasks/{TestTaskRecordId}/done", ahkResult);
                var patchNotFoundResponse = await scope.HttpClient.TryPatch<api.Model.Task>($"/api/tasks/{randomint}/done", ahkResult, allowNotFound: true);
                if (patchExistingResponse.Success && patchNotFoundResponse.Success)
                {
                    if (patchExistingResponse.Value.Title.Equals(TestTaskRecordTitle, StringComparison.OrdinalIgnoreCase) && patchExistingResponse.Value.Done
                        && patchNotFoundResponse.Value == null)
                    {
                        ahkResult.AddPoints(3);
                        ahkResult.Log("PATCH /api/tasks/id/done ok");
                    }
                    else
                    {
                        ahkResult.AddProblem("PATCH /api/tasks/id/done valasz tartalma hibas. PATCH /api/tasks/id/done yields invalid result.");
                    }
                }
            }
        }

        /// <summary>
        /// PATCH /api/tasks/{id}/move?newStatusName=newname
        /// </summary>
        private static async Task testMoveToNewStatus(AhkResult ahkResult)
        {
            using (var scope = WebAppInit.GetRequestScope())
            {
                string newStatusName = Guid.NewGuid().ToString();

                var patchResponse = await scope.HttpClient.TryPatch<api.Model.Task>($"/api/tasks/{TestTaskRecordId}/move?newStatusName={newStatusName}", ahkResult);
                if (patchResponse.Success)
                {
                    if (patchResponse.Value.Title.Equals(TestTaskRecordTitle, StringComparison.OrdinalIgnoreCase) && patchResponse.Value.Status == newStatusName)
                    {
                        ahkResult.AddPoints(3);
                        ahkResult.Log("PATCH /api/tasks/id/move ok");
                    }
                    else
                    {
                        ahkResult.AddProblem("PATCH /api/tasks/id/move valasz tartalma hibas. PATCH /api/tasks/id/move yields invalid result.");
                    }
                }
            }
        }
    }
}
