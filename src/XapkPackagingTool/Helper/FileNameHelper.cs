using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Helper
{
    internal static class FileNameHelper
    {
        public static string CleanFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();

            return string.Concat(fileName.Where(ch => !invalidChars.Contains(ch)));
        }

        public static bool IsValidFileName(string fileName)
        {
            return !fileName.Any(ch => Path.GetInvalidFileNameChars().Contains(ch));
        }

        public static string CreateAvaliableFileName(string fileName, string extension)
        {
            return $"{CleanFileName(fileName)}{extension}";
        }
    }
}
