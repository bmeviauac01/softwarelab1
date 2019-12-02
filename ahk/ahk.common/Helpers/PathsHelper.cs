using System.IO;
using System.Linq;

namespace ahk.common
{
    public static class PathsHelper
    {
        public static string FindFileWithCaseInsensitiveNameMatch(string filePath, ref AhkResult result)
        {
            var absolutePath = Path.GetFullPath(filePath);

            var parentDirPath = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(parentDirPath))
                return absolutePath;

            var matchedFiles = Directory.EnumerateFiles(parentDirPath)
                                .Where(f => f.ToUpperInvariant() == absolutePath.ToUpperInvariant())
                                .ToList();

            if (matchedFiles.Count == 1)
            {
                return matchedFiles[0];
            }
            else if (matchedFiles.Count > 1)
            {
                result.AddProblem($"Tobb azonos nevu fajl: {filePath}");
                return matchedFiles[0];
            }
            else
            {
                return absolutePath;
            }
        }
    }
}
