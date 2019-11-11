using System.IO;

namespace adatvez
{
    public static class PathsHelper
    {
        public static string GetFileInWellKnownDirectoryWithCaseInsensitiveNameMatching(string filePath)
        {
            var parentDirPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(parentDirPath))
                return filePath;

            foreach (var f in Directory.EnumerateFiles(parentDirPath))
            {
                if (f.ToUpperInvariant() == filePath.ToUpperInvariant())
                    return f;
            }

            return filePath;
        }
    }
}
