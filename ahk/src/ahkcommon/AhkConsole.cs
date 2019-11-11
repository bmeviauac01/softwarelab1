using System;
using System.Collections.Generic;
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

        public static void CheckSchreenshotAndWriteResult(string screenshotFilePath, string exerciseName, int points, params string[] comments)
        {
            var commentsList = new List<string>(comments);
            CheckSchreenshotAndWriteResult(screenshotFilePath, exerciseName, points, commentsList);
        }

        public static void CheckSchreenshotAndWriteResult(string screenshotFilePath, string exerciseName, int points, List<string> comments)
        {
            var screenshotOk = ScreenshotValidator.IsScreenshotPresent(screenshotFilePath, ref comments);

            if (!screenshotOk && points > 0)
            {
                // Hallgato nem adott be screenshotot, de legalabb reszben jo a megoldasa -> kezzel ellenorizzuk
                Inconclusive(exerciseName, comments.ToArray());
            }
            else if (screenshotOk && points == 0)
            {
                // Hallgato beadott screenshotot, de rossz a megoldasa -> kezzel ellenorizzuk
                Inconclusive(exerciseName, comments.ToArray());
            }
            else if (!screenshotOk && points == 0)
            {
                // Nincs screenshot, rossz megoldas -> valoszinuleg nem megoldott feladat, eredmenyt kiirjuk
                WriteResult(exerciseName, points, comments.ToArray());
            }
            else if (screenshotOk && points > 0)
            {
                // Screenshot is van es a megoldas is reszben jo -> eredmenyt kiirjuk
                WriteResult(exerciseName, points, comments.ToArray());
            }
        }
    }
}
