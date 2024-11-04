/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;

namespace XapkPackagingTool.Common.Helpers.FileHelpers
{
    public static class FileNameHelper
    {
        public static string GetUniqueFileName(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                return filePath;

            return GenerateUniqueFileName(filePath);
        }

        private static string GenerateUniqueFileName(string filePath)
        {
            string directory =
                Path.GetDirectoryName(filePath)
                ?? throw new InvalidOperationException("Directory name cannot be determined.");

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int count = 1;
            while (true)
            {
                string newFileName = $"{fileNameWithoutExtension}_{count}{extension}";
                string newFilePath = Path.Combine(directory, newFileName);

                if (!File.Exists(newFilePath))
                    return newFilePath;
                count++;
            }
        }
    }
}
