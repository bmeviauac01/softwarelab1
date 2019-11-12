using System.IO;

namespace adatvez
{
    public static class TextFileHelper
    {
        public static bool TryReadTextFileFromWellKnownPath(string fileDescription, string fileFullPath, ref AhkResult result, out string fileContent)
        {
            fileFullPath = PathsHelper.GetFileInWellKnownDirectoryWithCaseInsensitiveNameMatching(fileFullPath);

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
