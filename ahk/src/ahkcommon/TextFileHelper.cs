using System.Collections.Generic;
using System.IO;

namespace adatvez
{
    public static class TextFileHelper
    {
        public static bool TryReadTextFileFromWellKnownPath(string fileFullPath, string fileDescription, ref List<string> problems, out string fileContent)
        {
            fileFullPath = PathsHelper.GetFileInWellKnownDirectoryWithCaseInsensitiveNameMatching(fileFullPath);

            if (!File.Exists(fileFullPath))
            {
                problems.Add($"Nem talalhato {fileDescription} fajl");
                fileContent = null;
                return false;
            }

            fileContent = File.ReadAllText(fileFullPath);

            if (string.IsNullOrEmpty(fileContent))
            {
                problems.Add($"Ures {fileDescription} fajl");
                return false;
            }

            return true;
        }
    }
}
