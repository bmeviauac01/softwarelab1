using System;
using System.Linq;

namespace adatvez
{
    public static class AhkConsole
    {
        static readonly string validationCode;

        static AhkConsole()
        {
            validationCode = Environment.GetEnvironmentVariable("AHK_TEST_APP_CONSOLE_LOGGER_KEY");
        }

        public static void Fail(string exerciseName, params string[] comments)
            => writeResult(exerciseName, "0", comments);

        public static void Inconclusive(string exerciseName, params string[] comments)
            => writeResult(exerciseName, "inconclusive", comments);

        public static void WriteResult(string exerciseName, int points, params string[] comments)
            => writeResult(exerciseName, points.ToString(), comments);

        private static void writeResult(string exerciseName, string resultText, params string[] comments)
        {
            var outputLine = "###ahk#" + validationCode + "#testresult#" + exerciseName.Replace("#", "-") + "#" + resultText;

            if (comments != null && comments.Any())
                outputLine += "#" + string.Join("\\\n", comments.Select(c => c.Replace("\r\n", "\\\n").Replace("\n", "\\\n").Replace("\r", "\\\n")).ToArray());

            Console.WriteLine(outputLine);
        }

        public static void WriteResult(AhkResult result)
            => WriteResult(result.ExerciseName, result.Points, result.Problems.ToArray());

        public static void CheckSchreenshotAndWriteResult(string screenshotFilePath, ref AhkResult result)
        {
            var screenshotOk = ScreenshotValidator.IsScreenshotPresent(System.IO.Path.GetFileName(screenshotFilePath), screenshotFilePath, ref result);

            if (!screenshotOk && result.Points > 0)
            {
                // Hallgato nem adott be screenshotot, de legalabb reszben jo a megoldasa -> kezzel ellenorizzuk
                Inconclusive(result.ExerciseName, result.Problems.ToArray());
            }
            else if (screenshotOk && result.Points == 0)
            {
                // Hallgato beadott screenshotot, de rossz a megoldasa -> kezzel ellenorizzuk
                Inconclusive(result.ExerciseName, result.Problems.ToArray());
            }
            else if (!screenshotOk && result.Points == 0)
            {
                // Nincs screenshot, rossz megoldas -> valoszinuleg nem megoldott feladat, eredmenyt kiirjuk
                WriteResult(result.ExerciseName, result.Points, result.Problems.ToArray());
            }
            else if (screenshotOk && result.Points > 0)
            {
                // Screenshot is van es a megoldas is reszben jo -> eredmenyt kiirjuk
                WriteResult(result.ExerciseName, result.Points, result.Problems.ToArray());
            }
        }
    }
}
