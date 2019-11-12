using System;
using System.IO;

namespace adatvez
{
    public static class ScreenshotValidator
    {
        private static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        private const string knownHash = "CB96165DFFE7D1BBA338C1C0C99A8B6D";

        public static bool IsScreenshotPresent(string fileDescription, string filePath, ref AhkResult result)
        {
            if (string.IsNullOrEmpty(fileDescription))
                fileDescription = @"Kepernyokep fajl";
            else
                fileDescription = fileDescription.Trim();

            filePath = PathsHelper.GetFileInWellKnownDirectoryWithCaseInsensitiveNameMatching(filePath);

            if (!File.Exists(filePath))
            {
                result.AddProblem($"{fileDescription} hianyzik.");
                return false;
            }

            using (var fs = File.OpenRead(filePath))
            {
                var hash = md5.ComputeHash(fs);
                if (BitConverter.ToString(hash).Replace("-", "") == knownHash)
                {
                    result.AddProblem($"{fileDescription} nem lett felulirva.");
                    return false;
                }
            }

            result.Log($"{fileDescription} letezik");
            return true;
        }
    }
}
