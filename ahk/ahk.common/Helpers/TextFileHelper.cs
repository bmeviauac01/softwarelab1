using System.IO;

namespace ahk.common
{
    public static class TextFileHelper
    {
        public static bool TryReadTextFile(string fileDescription, string fileFullPath, ref AhkResult result, out string fileContent)
        {
            fileFullPath = PathsHelper.FindFileWithCaseInsensitiveNameMatch(fileFullPath, ref result);

            if (!File.Exists(fileFullPath))
            {
                result.AddProblem($"Nem talalhato {fileDescription} fajl");
                fileContent = null;
                return false;
            }

            fileContent = File.ReadAllText(fileFullPath);

            if (string.IsNullOrEmpty(fileContent))
            {
                result.AddProblem($"Ures {fileDescription} fajl");
                return false;
            }

            result.Log($"{fileDescription} fajl beolvasva");
            return true;
        }
    }
}
