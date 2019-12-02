using System;

namespace ahk.common
{
    public static class AhkExecutionHelper
    {
        public static int Execute(params Tuple<string, Action>[] checksToExecute)
        {
            Console.WriteLine("Teszteles indul...");

            if (checksToExecute == null || checksToExecute.Length == 0)
            {
                AhkResultWriter.Inconclusive("N/A", "Hibas teszt alklamazas");
                return -1;
            }

            try
            {
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
                AhkResultWriter.Inconclusive(ahkExerciseName, "Nem vart hiba a kiertekeles kozben");
                throw;
            }
        }
    }
}
