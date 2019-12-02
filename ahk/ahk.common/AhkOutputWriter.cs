using System.Linq;

namespace ahk.common
{
    public static class AhkOutputWriter
    {
        private static readonly string outputFilePath = @"result.txt";

        static AhkOutputWriter()
        {
            var environmentOutputFilePath = System.Environment.GetEnvironmentVariable("AHK_OUTPUTFILEPATH");
            if (!string.IsNullOrEmpty(environmentOutputFilePath))
                outputFilePath = environmentOutputFilePath;
        }

        public static void WriteToFile(this AhkResult result)
        {
            var str = result.ToOutputString();
            System.IO.File.AppendAllText(outputFilePath, str);
        }

        public static string ToOutputString(this AhkResult result)
        {
            var output = "###ahk#" + formatExerciseName(result.ExerciseName) + "#" + result.Points.ToString();

            if (result.Problems.Count > 0)
                output += "#" + string.Join("\\\n", result.Problems.Select(formatMultilineOutput).ToArray());

            return output;
        }

        public static void WriteInconclusiveResult(string exerciseName, string errorText)
        {
            var str = "###ahk#" + exerciseName.Replace("#", "-") + "#" + "inconclusive" + "#" + formatMultilineOutput(errorText);
            System.IO.File.AppendAllText(outputFilePath, str);
        }

        private static string formatExerciseName(string exerciseName)
            => exerciseName.Replace("#", "-");

        private static string formatMultilineOutput(string c)
            => c.Replace("\r\n", "\\\n").Replace("\n", "\\\n").Replace("\r", "\\\n");
    }
}
