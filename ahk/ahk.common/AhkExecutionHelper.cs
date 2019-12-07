using System;

namespace ahk.common
{
    public static class AhkExecutionHelper
    {
        public static int Execute(params AhkEvaluationTask[] tasksToExecute)
        {
            Console.WriteLine("Kiertekeles indul...");

            if (tasksToExecute == null || tasksToExecute.Length == 0)
            {
                AhkOutputWriter.WriteInconclusiveResult("N/A", "Hibas teszt alkalmazas. Error in the evaluation application.");
                return -1;
            }

            try
            {
                foreach (var test in tasksToExecute)
                    executeSafe(test);

                Console.WriteLine("Kiertekeles befejezve.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kiertekelesben hiba tortent.");
                Console.WriteLine(ex);
                return -1;
            }
        }

        private static void executeSafe(AhkEvaluationTask task)
        {
            var result = new AhkResult(task.ExerciseName);
            try
            {
                task.ExecuteAction(result);

                if (!task.IsPreProcess)
                    result.WriteToFile();
            }
            catch (Exception ex)
            {
                AhkOutputWriter.WriteInconclusiveResult(task.ExerciseName, "Nem vart hiba a kiertekeles kozben");

                Console.WriteLine("Kiertekelesben hiba tortent.");
                Console.WriteLine(ex);

                if (task.IsPreProcess)
                    throw;
            }
        }
    }
}
