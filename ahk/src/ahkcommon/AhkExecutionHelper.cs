using System;

namespace adatvez
{
    public static class AhkExecutionHelper
    {
        public static int Execute(bool initializeDatabase = true, params Tuple<string, Action>[] checksToExecute)
        {
            Console.WriteLine("Teszteles indul...");

            if (checksToExecute == null || checksToExecute.Length == 0)
            {
                AhkConsole.Inconclusive("N/A", "Hibas teszt alklamazas");
                return -1;
            }

            try
            {
                var firstTaskName = checksToExecute[0].Item1;
                if (initializeDatabase)
                    executeSafe(firstTaskName, DbInit.InitializeDatabase);

                foreach (var test in checksToExecute)
                    executeSafe(test.Item1, test.Item2);

                Console.WriteLine("Sikeres teszt lefutas.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba tortent.");
                Console.WriteLine(ex);
                return -1;
            }
        }

        private static void executeSafe(string ahkExerciseName, Action action)
        {
            try
            {
                action();
            }
            catch
            {
                AhkConsole.Inconclusive(ahkExerciseName, "Nem vart hiba a kiertekeles kozben");
                throw;
            }
        }
    }
}
